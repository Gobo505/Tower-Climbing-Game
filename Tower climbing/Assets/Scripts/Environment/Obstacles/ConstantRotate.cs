using UnityEngine;

namespace Assets.Scripts.Environment.Obstacles {

    enum RotateDirection {
        RIGHT, LEFT
    }

    public class ConstantRotate : MonoBehaviour {

        [SerializeField]
        [Tooltip("Minimum object rotation speed")]
        private float rotationSpeedMin = 200;

        [SerializeField]
        [Tooltip("Maximum object rotation speed")]
        private float rotationSpeedMax = 350;

        [SerializeField]
        [Tooltip("Direction in which object will rotate")]
        private RotateDirection rotateDirection;

        private float rotationSpeed;

        private void Start() {
            rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        }

        public void Update() {
            if(rotateDirection == RotateDirection.LEFT) {
                transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            } else {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
            }
        }

    }
}
