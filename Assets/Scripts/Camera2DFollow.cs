using UnityEngine;

namespace Runner
{
    /*modified version of Standard Assets : UnityStandardAssets._2D.Camera2DFollow
    */


    public class Camera2DFollow : MonoBehaviour
    {
        public float yAxisOffset = 2f;
        public float yOffsetPeekUpDown;
        private Vector3 targetPosition;
        private float _yAxisOffset;

        public Transform target;
        public float damping = 0.5f;
        public float lookAheadFactor = 1;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private float orthographicSize;
        private void Start()
        {
            _yAxisOffset = yAxisOffset;
            targetPosition = Vector3.zero;
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;

            orthographicSize = transform.GetComponent<Camera>().orthographicSize;
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateYAxisOffset();
            UpdatePosition();
            UpdateCameraSize();
        }

        private void UpdatePosition()
        {
            targetPosition = target.position + new Vector3(2, yAxisOffset, 0);

            float xMoveDelta = (targetPosition - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * xMoveDelta;
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = targetPosition + m_LookAheadPos + Vector3.forward * m_OffsetZ;

            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }

        private void UpdateCameraSize()
        {
            float vSpeed = target.GetComponent<Rigidbody2D>().velocity.y;
            if (vSpeed != 0.0f)
            {

                float newSize = transform.GetComponent<Camera>().orthographicSize + (vSpeed * 0.005f);
                if (newSize > 0)
                {
                    transform.GetComponent<Camera>().orthographicSize = newSize;
                }
            }
            else
            {
                transform.GetComponent<Camera>().orthographicSize = orthographicSize;
            }
        }

        private void UpdateYAxisOffset()
        {
            if (Input.GetKeyDown("down"))
            {
                yAxisOffset = yOffsetPeekUpDown - _yAxisOffset;

            }
            else if (Input.GetKeyDown("up"))
            {
                yAxisOffset = yOffsetPeekUpDown + _yAxisOffset;

            }
            else if (Input.GetKeyUp("down"))
            {

                yAxisOffset = _yAxisOffset;

            }
            else if (Input.GetKeyUp("up"))
            {
                yAxisOffset = _yAxisOffset;

            }
        }
    }
}