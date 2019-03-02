using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using Photon.MmoDemo.Common;

namespace Servers.Models.Interfaces
{
    public interface ICharacter
    {
        int Health { get; set; }
        Vector Position { get; set; }
        bool IsTeleporting { get; }
        bool IsDead { get; }
        IList<ICharacter> StatusListeners { get; }
        IList<IStat> Stats { get; }
        int AttackCooldown { get; }
        ICharacter AttackableTarget { get; }
        void OffsetHealth(int offset, ICharacter attacker, bool notify = true);
        
        RelationshipType RelationshipWith(ICharacter character);

        // Cooldowns

        // Combat related
        bool Attack(ICharacter target);
        void ReceiveAttack(ICharacter attacker, int damage);
        bool StartAutoAttack();
        void StopAutoAttack();

        // Movement and position related
        void StartFollowing(ICharacter obj, Action onArrive);
        void StopFollowing();
        bool IsMoving();
        bool IsAbleToMove();
        void StopMove(Position pos);
        void MoveTo(Position pos, Action onArrive = null);
        void MoveTo(IObject obj, Action onArrive = null);
        bool UpdateMovement(int tick);
        void Teleport(Position position);
        void Teleport(float x, float y, float z, short heading);
        void Teleport(float x, float y, float z);

        bool Die(ICharacter killer);
        void CalculateRewards(ICharacter killer);
        void UpdateBroadcastStatus(int broadcastType);
        void Reset();

        // Combat
        bool IsAggressive();
        bool IsInCombat();
        bool IsAttackingDisabled();
    }
}

