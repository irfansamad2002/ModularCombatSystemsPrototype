using Project.Systems.Effects;

public class SlowInstance : EffectInstance
{
    private float _slowMultiplier;
    private SimpleEnemyAI _ai;

    public SlowInstance(float slowMultiplier, float duration)
    {
        _slowMultiplier = slowMultiplier;
        _duration = duration;
    }

    protected override void OnStart()
    {
        _ai = _target.GetComponent<SimpleEnemyAI>();
        if (_ai == null)
        {
            DebugHelper.WarnMissingComponent(_target, nameof(SimpleEnemyAI));
            return;
        }

        _ai.ApplySlow(_slowMultiplier);
    }

    protected override void OnTick(float deltaTime)
    {
        //nth on tick
    }

    public override void OnReapply(EffectInstance newInstance)
    {
        _timer = 0f;
    }

    public override void OnEnd()
    {
        if (_ai == null)
        {
            DebugHelper.WarnMissingComponent(_target, nameof(SimpleEnemyAI));
            return;
        }

        _ai.ResetSpeed();
        //restore speed
        //_ai.currentMoveSpeed = _ai.baseMoveSpeed;
    }
}
