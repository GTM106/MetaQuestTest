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

        //つかむ処理
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

        //物体を離す処理
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            foreach (var item in rbs)
            {
                item.useGravity = true;

#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                item.velocity = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RHand);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
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
            //生成した球の位置を右手コントローラの位置に変更
            go.transform.position = rightController.transform.position;

            sphere.AddForce(rightController.transform.forward * 10.0f, ForceMode.Impulse);
            isPressedAButton = false;
        }

    }
}

