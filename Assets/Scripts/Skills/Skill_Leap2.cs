using UnityEngine;
using UnityEngine.Events;

public class Skill_Leap2 : Skill
{
    [SerializeField] private string targetLayerName;
    [SerializeField] private Vector2 dmgRectangleSize;
    [SerializeField] private Vector2 dmgRectangleOffset;
    [SerializeField] private float dmgAmount;
    [SerializeField] private UnityEvent<float> OnTargetsContactEvent; // dmgAmount

    protected override void Start()
    {
        pStat.OnLandEvent.AddListener(OnLand);
    }

    private void OnLand()
    {
        if (!this.enabled || skillState != SkillState.Waiting) return;

        RaycastHit2D[] hitList =
            Physics2D.BoxCastAll(transform.position + (Vector3)dmgRectangleOffset, dmgRectangleSize, 0, Vector2.zero, 0, LayerMask.GetMask(targetLayerName));

        if (hitList.Length != 0)
            foreach (RaycastHit2D hit in hitList)
            {
                Health_Handler colHealth;
                hit.collider.TryGetComponent<Health_Handler>(out colHealth);

                if (colHealth != null)
                {
                    colHealth.Public_DecreaseHealth(dmgAmount);
                    OnTargetsContactEvent?.Invoke(dmgAmount);
                }
            }

        pStat.Public_ResetGravity();
        OnSkillEnd();
    }
}
