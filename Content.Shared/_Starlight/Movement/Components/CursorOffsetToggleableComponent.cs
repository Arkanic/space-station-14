using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;

namespace Content.Shared._Starlight.Movement.Components;

/// <summary>
/// An action interaction for toggling the cursor offset view
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CursorOffsetToggleableComponent : Component
{
    [DataField]
    public EntProtoId Action = "ActionToggleViewOffset";

    public EntityUid? ActionEntity;

    /// <summary>
    /// Currently in "bino view"?
    /// </summary>
    public bool Looking = false;
}