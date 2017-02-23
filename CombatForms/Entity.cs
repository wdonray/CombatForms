using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CombatForms
{

    [Serializable]
    [XmlInclude(typeof(Entity))]
    public class Entity : IDamageable, IDamager
    {
        [XmlElement(ElementName = "CurrentState")]
        public State CurrentState { get; set; }
        [XmlElement(ElementName = "PlayerFSM")]
        public FiniteStateMachine<PlayerStates> fsm;
        public enum EType
        {
            PLAYER,
            ENEMY,
        }
        public EType eType;
        public Entity() { }
        public Entity(float health, string name, bool alive, bool block, float speed, EType e, FiniteStateMachine<PlayerStates> f)
        {
            fsm = f;
            fsm.Start(PlayerStates.INIT);
            CurrentState = fsm.currentState;
            Health = health;
            Name = name;
            Alive = alive;
            Speed = speed;
            IsBlocking = block;
            eType = e;
            LevelUp = 1;
            Exp = 0;
            MaxExp = 50;
            MaxHealth = 100;
        }
        public string Space = "-------------------------------------------------------------";
        /// <summary>
        /// Does damage based on if the selected target is blocking or not and adds log to combatLog
        /// </summary>
        /// <param name="d"></param>
        public void DoDamage(IDamageable d)
        {
            Random level = new Random();
            Random rand = new Random();
            Random crit = new Random();
            float damage = rand.Next(10, 16);
            if (d.IsBlocking == false)
            {
                float critChance = crit.Next(1, 101);
                //Added a crit chance of 15%
                if (critChance <= 15)
                {
                    damage = damage * 2;
                    d.TakeDamage(damage);
                    Combat.Instance.combatLog += this.Name + " is attacking and CRIT "
                         + (d as Entity).Name + " for " + damage.ToString() + " damage!" + Environment.NewLine + Space + Environment.NewLine;
                    Combat.Instance.CV.ActiveParty.ActivePlayer.AddExp(level.Next(25, 71));
                    if (Combat.Instance.CV.ActiveParty.ActivePlayer.Exp >= Combat.Instance.CV.ActiveParty.ActivePlayer.MaxExp)
                        Combat.Instance.CV.ActiveParty.ActivePlayer.levelUp();
                }

                else
                {
                    d.TakeDamage(damage);
                    Combat.Instance.combatLog += this.Name + " is attacking "
                        + (d as Entity).Name + " for " + damage.ToString() + " damage!" + Environment.NewLine + Space + Environment.NewLine;
                    Combat.Instance.CV.ActiveParty.ActivePlayer.AddExp(level.Next(20, 51));
                    if (Combat.Instance.CV.ActiveParty.ActivePlayer.Exp >= Combat.Instance.CV.ActiveParty.ActivePlayer.MaxExp)
                        Combat.Instance.CV.ActiveParty.ActivePlayer.levelUp();
                }
            }
            else if (d.IsBlocking == true)
            {
                damage = damage / 2;
                d.TakeDamage(damage);
                Combat.Instance.combatLog += this.Name + " is attacking "
                   + (d as Entity).Name + "(Blocked half the damage)" + " for " + damage.ToString() + " damage!" + Environment.NewLine + Space + Environment.NewLine;
                d.IsBlocking = false;
                Combat.Instance.CV.ActiveParty.ActivePlayer.AddExp(level.Next(15, 31));
                if (Combat.Instance.CV.ActiveParty.ActivePlayer.Exp >= Combat.Instance.CV.ActiveParty.ActivePlayer.MaxExp)
                    Combat.Instance.CV.ActiveParty.ActivePlayer.levelUp();
            }
        }
        /// <summary>
        /// Sets a selected entity to set damage
        /// </summary>
        /// <param name="f"></param>
        public void TakeDamage(float f)
        {
            Health -= f;
            if (Health <= 0)
            {
                Alive = false;
                Health = 0;
                Combat.Instance.combatLog += this.Name + " has died" + Environment.NewLine + Space + Environment.NewLine;
            }
        }
        /// <summary>
        /// Invokes the end turn delegate
        /// </summary>
        public void EndTurn()
        {
            if (fsm.ChangeState(PlayerStates.REST) == false)
                return;
            CurrentState = fsm.GetState();
            if (onEndTurn == null)
                throw new NullReferenceException("Please give me a function to execute");
            if (onEndTurn != null)
                onEndTurn.Invoke();
        }
        /// <summary>
        /// Checks the current eType and attacks a random opposing target
        /// </summary>
        public void Attack()
        {
            if (fsm.ChangeState(PlayerStates.ATTACK) == false)
                return;
            CurrentState = fsm.GetState();
            Random random = new Random();
            int targetP = random.Next(0, (Combat.Instance.CV.PlayerParty.members.Count));
            int targetE = random.Next(0, (Combat.Instance.CV.EnemyParty.members.Count));
            if (this.eType == EType.ENEMY && Combat.Instance.CV.ActiveParty.ActivePlayer.Alive == true)
            {
                do
                {
                    targetP = random.Next(0, (Combat.Instance.CV.PlayerParty.members.Count));
                } while (Combat.Instance.CV.PlayerParty.members[targetP].Alive == false);
                DoDamage(Combat.Instance.CV.PlayerParty.members[targetP]);
            }
            else if (this.eType == EType.PLAYER && Combat.Instance.CV.ActiveParty.ActivePlayer.Alive == true)
            {
                do
                {
                    targetE = random.Next(0, (Combat.Instance.CV.EnemyParty.members.Count));
                } while (Combat.Instance.CV.EnemyParty.members[targetE].Alive == false);
                DoDamage(Combat.Instance.CV.EnemyParty.members[targetE]);
            }
        }
        /// <summary>
        /// Sets the current player to take its remaining HP as damage
        /// </summary>
        public void Flee()
        {
            if (fsm.ChangeState(PlayerStates.FLEE) == false)
                return;
            CurrentState = fsm.GetState();
            Combat.Instance.CV.ActiveParty.ActivePlayer.Alive = false;
            if (Combat.Instance.CV.ActiveParty.ActivePlayer.Alive == false)
            {
                Combat.Instance.CV.ActiveParty.ActivePlayer.Health = 0;
                Combat.Instance.combatLog += this.Name + " has died" + Environment.NewLine + Space + Environment.NewLine;
            }
        }
        /// <summary>
        /// Sets the current player boolean (isBlocking) to true and adds text to the combatLog String
        /// </summary>
        public void Defend()
        {
            if (fsm.ChangeState(PlayerStates.DEFEND) == false)
                return;
            CurrentState = fsm.GetState();
            Combat.Instance.CV.ActiveParty.ActivePlayer.IsBlocking = true;
            Combat.Instance.combatLog += this.Name + " prepared a block! " + Environment.NewLine + Space + Environment.NewLine;
        }
        /// <summary>
        /// Math to level up
        /// </summary>
        public void levelUp()
        {
            LevelUp++;
            Speed++;
            Exp -= (int)MaxExp;
            MaxExp = (int)(Math.Pow((double)50, (double)(LevelUp + 2) / (double)5) + (double)50);
            MaxHealth = (int)(Math.Pow((double)100, (double)(LevelUp + 1) / (double)5) + (double)100);
            Health += 10;
        }
        /// <summary>
        /// Function to add desired exp to my current interger
        /// </summary>
        /// <param name="E"></param>
        public void AddExp(int E)
        {
            Exp += E;
        }

        public delegate void Handler();
        [XmlIgnore]
        public Handler onEndTurn;
        /// <summary>
        /// The representaion if the Entitie is alive.
        /// </summary>
        public bool Alive
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsBlocking
        {
            get;
            set;
        }
        /// <summary>
        /// The represenation of an Entities Life Total.
        /// If that life total ever gets less than zero we got a problem or dude is dead.
        /// </summary>
        public float Health
        {
            get;
            private set;
        }
        /// <summary>
        /// The representation of an Entities string name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }
        /// <summary>
        /// The representation of an Entities Speed.
        /// Using this to sort the Entities in order.
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }
        /// <summary>
        /// The representation of an Entities Exp.
        /// If reaches the Max EXP reset it to 0.
        /// </summary>
        public int Exp
        {
            get;
            set;
        }
        /// <summary>
        /// Using the leveling algorithm changes as it reaches 100%.
        /// </summary>
        public int MaxExp
        {
            get;
            set;
        }
        /// <summary>
        /// Used in the leveling alogrithm
        /// </summary>
        public int LevelUp
        {
            get;
            set;
        }
        /// <summary>
        /// Using the leveling algorithm changes as it reaches 100%.
        /// </summary>
        public int MaxHealth
        {
            get;
            set;
        }
    }
}
