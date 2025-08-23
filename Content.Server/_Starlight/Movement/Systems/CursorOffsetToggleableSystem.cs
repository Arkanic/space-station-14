using Content.Shared._Starlight.Movement.Systems;
using Content.Shared._Starlight.Movement.Components;
using Content.Shared.Toggleable;
using Content.Shared.Mobs;
using Content.Shared.Camera;
using Content.Server.Actions;
using Content.Server.Movement.Systems;
using Content.Server.Movement.Components;

namespace Content.Server._Starlight.Movement.Systems;

public sealed class CursorOffsetToggleableSystem : SharedCursorOffsetToggleableSystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly ContentEyeSystem _eye = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CursorOffsetToggleableComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<CursorOffsetToggleableComponent, ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<CursorOffsetToggleableComponent, ToggleActionEvent>(OnLookingToggle);
        SubscribeLocalEvent<CursorOffsetToggleableComponent, MobStateChangedEvent>(OnMobStateChanged);
        SubscribeLocalEvent<CursorOffsetToggleableComponent, GetEyePvsScaleEvent>(OnGetEyePvsScale);
    }

    private void OnComponentInit(EntityUid uid, CursorOffsetToggleableComponent component, ComponentInit args)
    {
        _actions.AddAction(uid, ref component.ActionEntity, component.Action, uid);
    }

    private void OnComponentShutdown(EntityUid uid, CursorOffsetToggleableComponent component, ComponentShutdown args)
    {
        _actions.RemoveAction(uid, component.ActionEntity);
    }

    private void OnLookingToggle(EntityUid uid, CursorOffsetToggleableComponent component, ref ToggleActionEvent args)
    {
        if (args.Handled) return;

        ToggleLooking(uid, component);
    }

    private void OnMobStateChanged(EntityUid uid, CursorOffsetToggleableComponent component, MobStateChangedEvent args)
    {
        if (component.Looking) ToggleLooking(uid, component); // don't want to be stuck in goofy far see mode on crit/death
    }

    public void ToggleLooking(EntityUid uid, CursorOffsetToggleableComponent component)
    {
        component.Looking = !component.Looking;

        _eye.UpdatePvsScale(uid);

        
    }

    private void OnGetEyePvsScale(EntityUid uid, CursorOffsetToggleableComponent component, ref GetEyePvsScaleEvent args)
    {
        if (!TryComp<EyeCursorOffsetComponent>(uid, out var eyeCursorOffset)) return;

        if (!component.Looking) return;

        args.Scale += eyeCursorOffset.PvsIncrease;
    }
}