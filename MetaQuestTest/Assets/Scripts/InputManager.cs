using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject rightController;
    [SerializeField]
    GameObject leftController;

    [SerializeField] Rigidbody sphere;

    bool isPressedAButton;
    bool isReleasedTriggerButton;

    readonly List<Rigidbody> rbs = new();

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            isPressedAButton = true;
        }

        //���ޏ���
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(rightController.transform.position, 0.01f, Vector3.forward);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Cube"))
                {
                    hit.collider.transform.parent = rightController.transform;
                    hit.collider.attachedRigidbody.useGravity = false;
                    hit.collider.isTrigger = true;
                    rbs.Add(hit.collider.attachedRigidbody);
                }
            }
        }


        foreach (var item in rbs)
        {
            item.velocity = Vector3.zero;
        }

        //���̂𗣂�����
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            foreach (var item in rbs)
            {
                item.useGravity = true;

#pragma warning disable CS0618 // �^�܂��̓����o�[�����^���ł�
                item.velocity = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RHand);
#pragma warning restore CS0618 // �^�܂��̓����o�[�����^���ł�
                item.transform.parent = null;

                item.GetComponent<Collider>().isTrigger = false;
            }
            rbs.Clear();
        }
    }

    private void FixedUpdate()
    {
        if(isPressedAButton)
        {
            GameObject go = Instantiate(sphere.gameObject);
            //�����������̈ʒu���E��R���g���[���̈ʒu�ɕύX
            go.transform.position = rightController.transform.position;

            sphere.AddForce(rightController.transform.forward * 10.0f, ForceMode.Impulse);
            isPressedAButton = false;
        }

    }
}

