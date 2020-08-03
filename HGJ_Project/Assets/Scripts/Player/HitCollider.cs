using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public float knockbackStrength;
    public float spinTimeToInflict;

    public Transform mainBody;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitDetector")
        {
            HitDetector hd = other.GetComponent<HitDetector>();

            if (other.gameObject.name == "RightHit" || other.gameObject.name == "LeftHit")
            {
                Vector3 directions = hd.mainBody.position - mainBody.position;
                directions.y = 0;

                hd.player.spinTime = spinTimeToInflict;
                hd.player.isHit = true;
                hd.rb.AddForce(directions.normalized * knockbackStrength * 1.5f, ForceMode.Impulse);
            }
            else
            {
                Vector3 direction = hd.mainBody.position - mainBody.position;
                direction.y = 0;

                hd.rb.AddForce(direction.normalized * knockbackStrength / 1.5f, ForceMode.Impulse);
            }
        }

    }

}
