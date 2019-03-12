using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using Photon.MmoDemo.Common;
using Servers.Models.Interfaces;

namespace Servers.Models
{
    public class Character: ICharacter
    {
        public int Health { get; set; }
        public bool IsTeleporting { get; }
        public bool IsDead { get; }
        public IList<ICharacter> StatusListeners { get; }
        public IList<IStat> Stats { get; }
        public int AttackCooldown { get; }
        public ICharacter AttackableTarget { get; }

        public MGF.Domain.Character CharacterDataFromDb { get; set; } 

        public Vector Position { get; set; }

        public void OffsetHealth(int offset, ICharacter attacker, bool notify = true)
        {
            throw new NotImplementedException();
        }

        public RelationshipType RelationshipWith(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public bool Attack(ICharacter target)
        {
            throw new NotImplementedException();
        }

        public void ReceiveAttack(ICharacter attacker, int damage)
        {
            throw new NotImplementedException();
        }

        public bool StartAutoAttack()
        {
            throw new NotImplementedException();
        }

        public void StopAutoAttack()
        {
            throw new NotImplementedException();
        }

        public void StartFollowing(ICharacter obj, Action onArrive)
        {
            throw new NotImplementedException();
        }

        public void StopFollowing()
        {
            throw new NotImplementedException();
        }

        public bool IsMoving()
        {
            throw new NotImplementedException();
        }

        public bool IsAbleToMove()
        {
            throw new NotImplementedException();
        }

        public void StopMove(Position pos)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(Position pos, Action onArrive = null)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(IObject obj, Action onArrive = null)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMovement(int tick)
        {
            throw new NotImplementedException();
        }

        public void Teleport(Position position)
        {
            throw new NotImplementedException();
        }

        public void Teleport(float x, float y, float z, short heading)
        {
            throw new NotImplementedException();
        }

        public void Teleport(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public bool Die(ICharacter killer)
        {
            throw new NotImplementedException();
        }

        public void CalculateRewards(ICharacter killer)
        {
            throw new NotImplementedException();
        }

        public void UpdateBroadcastStatus(int broadcastType)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public bool IsAggressive()
        {
            throw new NotImplementedException();
        }

        public bool IsInCombat()
        {
            throw new NotImplementedException();
        }

        public bool IsAttackingDisabled()
        {
            throw new NotImplementedException();
        }
    }
}
