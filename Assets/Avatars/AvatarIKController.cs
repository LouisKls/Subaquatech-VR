using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AvatarIKController : MonoBehaviour
{
    private Animator animator;

    [Header("VR References (� assigner apr�s l'Instantiate)")]
    public Transform vrHead;       // Transform du casque (Main Camera du XR Origin)
    public Transform vrLeftHand;   // Transform du contr�leur gauche
    public Transform vrRightHand;  // Transform du contr�leur droit

    [Range(0f, 1f)]
    public float ikWeight = 1.0f;  // Intensit� de l'IK (0 = pas d'IK, 1 = full IK)

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // -- Fait tourner le corps pour qu'il fasse face � la direction "horizontale" du casque --
        if (vrHead != null)
        {
            Vector3 forwardDir = vrHead.forward;
            forwardDir.y = 0f;        // On ignore la composante verticale pour ne pas se pencher
            forwardDir.Normalize();
            transform.forward = forwardDir;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!animator) return;

        // -- HEAD : on utilise "LookAt" pour la t�te --
        if (vrHead != null)
        {
            // D�finir le poids (1er param) et �ventuellement comment on r�partit l'influence entre corps/t�te/yeux
            animator.SetLookAtWeight(
                /* weight     = */ ikWeight,
                /* bodyWeight = */ 0.3f,
                /* headWeight = */ 1f,
                /* eyesWeight = */ 0.0f,
                /* clampWeight= */ 0.5f
            );
            // La t�te regardera ce point
            animator.SetLookAtPosition(vrHead.position);
        }

        // -- LEFT HAND : AvatarIKGoal.LeftHand --
        if (vrLeftHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, vrLeftHand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, vrLeftHand.rotation);
        }

        // -- RIGHT HAND : AvatarIKGoal.RightHand --
        if (vrRightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, vrRightHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, vrRightHand.rotation);
        }
    }
}
