using FSM;
using FSM.States;
using UnityEngine;

namespace FSM.States.ActorS
{
    public sealed class AirborneState : State
    {
        private Actor m_actor = null;
        private float m_drag = 0f;

        public AirborneState(Actor actor, float drag, string name)
        {
            m_actor = actor;
            m_drag = drag;
            m_name = name;
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

        public IdleState(Actor actor, float friction, string name)
        {
            m_actor = actor;
            m_friction = friction;
            m_name = name;
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

        public WallSlideState(Actor actor, float slideSpeed, string name)
        {
            m_actor = actor;
            m_slideSpeed = slideSpeed;
            m_name = name;
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

    //public sealed class DodgeRollState : State
    //{
    //    private Player m_player = null;
    //    private DodgeRollConfig m_config;
    //    private Parameter m_trigger = null;
    //    private float m_startTime = 0f;


    //    public DodgeRollState(Player player, DodgeRollConfig config, Parameter trigger, string name)
    //    {
    //        m_player = player;
    //        m_config = config;
    //        m_trigger = trigger;
    //        m_name = name;
    //    }

    //    public override void OnStateEnter()
    //    {
    //        Vector2 aimDirection = m_player.GetAimDirection();
    //        int rollDirection = aimDirection.x > 0f ? 1 : -1;

    //        m_player.AnimSetTrigger("Dodge Roll");
    //        m_player.Knockback(Vector2.right * rollDirection * m_config.force, m_config.deadzoneTime);

    //        m_startTime = Time.time;

    //        Debug.Log("Dodge Roll State Enter!");
    //    }

    //    public override void OnStateExit()
    //    {

    //    }

    //    public override void OnStateUpdate()
    //    {
    //        if (Time.time >= m_startTime + m_config.deadzoneTime)
    //        {
    //            m_trigger.m_value = false;
    //        }
    //    }
    //}
}