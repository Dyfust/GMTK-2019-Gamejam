using FSM;
using FSM.States;
using UnityEngine;

namespace FSM.States.ActorS
{
    public sealed class AirborneState : State
    {
        private Actor m_actor = null;
        private float m_drag = 0f;

        public AirborneState(Actor actor, float drag)
        {
            m_actor = actor;
            m_drag = drag;
        }

        public override void OnStateEnter()
        {
            m_actor.SetResistance(m_drag);
            Debug.Log("Airborne State Enter!");
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {

        }
    }

    public sealed class IdleState : State
    {
        private Actor m_actor = null;
        private float m_friction = 0f;

        public IdleState(Actor actor, float friction)
        {
            m_actor = actor;
            m_friction = friction;
        }

        public override void OnStateEnter()
        {
            m_actor.SetResistance(m_friction);
            Debug.Log("Idle State Enter!");
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {

        }
    }

    public sealed class WallSlideState : State
    {
        private Actor m_actor = null;
        private float m_slideSpeed = 0f;

        public WallSlideState(Actor actor, float slideSpeed)
        {
            m_actor = actor;
            m_slideSpeed = slideSpeed;
        }

        public override void OnStateEnter()
        {
            Debug.Log("Wall Sliding State Enter!");
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            if (m_actor.GetVelocity().y < -m_slideSpeed)
            { m_actor.SetVelocity(m_actor.GetVelocity().x, -m_slideSpeed); }
        }
    }
}