using Fusion;
using UnityEngine;
using static Oculus.Avatar2.OvrAvatarEntity;

public class AvatarStateSync : NetworkBehaviour
{
    [SerializeField] AvatarEntityState avatarEntityState;

    [Networked] public ulong OculusID { get; set; }

    [Networked] private uint AvatarDataCount { get; set; }

    private const int AvatarDataSize = 1200;

    [Networked(OnChanged = nameof(OnAvatarDataChanged)), Capacity(AvatarDataSize)] private NetworkArray<byte> AvatarData { get; }

    private byte[] byteArray = new byte[AvatarDataSize];

    public override void Spawned()
    {
        if (Object.HasStateAuthority) OculusID = UserEntitlement.OculusID;
    }

    public void RecordAvatarState(StreamLOD streamLOD)
    {
        AvatarDataCount = avatarEntityState.RecordStreamData_AutoBuffer(streamLOD, ref byteArray);

        AvatarData.CopyFrom(byteArray, 0, byteArray.Length);
    }

    static void OnAvatarDataChanged(Changed<AvatarStateSync> changed) => changed.Behaviour.ApplyAvatarData();

    private void ApplyAvatarData()
    {
        if (Object.HasStateAuthority) return;

        var slicedData = new byte[AvatarDataCount];
        AvatarData.CopyTo(slicedData, throwIfOverflow: false);
        avatarEntityState.AddToDataBuffer(slicedData);
    }
}
