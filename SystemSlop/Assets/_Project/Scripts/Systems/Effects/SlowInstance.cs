using Project.Entities;
using Project.Systems.Effects;
using Project.VFX;

public class SlowInstance : EffectInstance
{
    private float _slowMultiplier;
    private SimpleEnemyAI _ai;
    private EnemyVisualFeedback _visual;

    public SlowInstance(float slowMultiplier, float duration)
    {
        _slowMultiplier = slowMultiplier;
        _duration = duration;
    }

    protected override void OnStart()
    {
        _ai = _target.GetComponent<SimpleEnemyAI>();
        _visual = _target.GetComponent<EnemyVisualFeedback>();

        if (_ai == null)
        {
            DebugHelper.WarnMissingComponent(_target, nameof(SimpleEnemyAI));
            return;
        }


        _ai.ApplySlow(_slowMultiplier);
        _visual?.SetSlowed(true);
    }

    protected override void OnTick(float deltaTime)
    {

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
        _visual?.SetSlowed(false);
    }
}
