using UnityEngine;

public static class Global
{
    public static readonly int GroundLayerIndex = LayerMask.NameToLayer("Ground");
    public static readonly LayerMask GroundLayer = LayerMask.GetMask("Ground");

    public static readonly int PlayerLayerIndex = LayerMask.NameToLayer("Player");
    public static readonly LayerMask PlayerLayer = LayerMask.GetMask("Player");
}
