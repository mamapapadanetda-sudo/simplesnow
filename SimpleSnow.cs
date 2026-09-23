using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace SimpleSnow;

public class SimpleSnow : BasePlugin
{
    public override string ModuleName => "Simple Snow";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "You";
    public override string ModuleDescription => "Просто снег с неба";

    private const string SnowParticle = "particles/goldkingz/snowing/snowing.vpcf";

    private CBaseEntity? _snowEntity;

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        SpawnSnow();
        return HookResult.Continue;
    }

    private void SpawnSnow()
    {
        // Удаляем старый, если есть
        if (_snowEntity != null && _snowEntity.IsValid)
        {
            _snowEntity.Remove();
            _snowEntity = null;
        }

        // Создаём партикл
        var particle = Utilities.CreateEntityByName<CInfoParticleSystem>("info_particle_system");
        if (particle == null) return;

        particle.EffectName = SnowParticle;
        particle.StartActive = true;

        // Ставим высоко над картой, чтобы снег падал сверху
        particle.Teleport(
            new Vector(0, 0, 1000),   // координаты (центр карты, высота)
            new QAngle(0, 0, 0),
            new Vector(0, 0, 0)
        );

        particle.DispatchSpawn();
        _snowEntity = particle;
    }

    public override void Unload(bool hotReload)
    {
        if (_snowEntity != null && _snowEntity.IsValid)
        {
            _snowEntity.Remove();
            _snowEntity = null;
        }
    }
}
