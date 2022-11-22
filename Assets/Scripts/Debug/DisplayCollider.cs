using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class DisplayCollider : MonoBehaviour {
    public bool AlwaysDraw;
    public bool DrawWire;
    public Color Color = Color.white;

    BoxCollider[] boxes;
    SphereCollider[] spheres;
    CapsuleCollider[] capsules;

    private void Awake() {
        PopulateColliderArrays();
    }

    private void OnDrawGizmos() {
        if (!AlwaysDraw) {
            return;
        }

        foreach (BoxCollider collider in boxes) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawBox(collider);
        }
        foreach (SphereCollider collider in spheres) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawSphere(collider);
        }
#if UNITY_EDITOR
        foreach (CapsuleCollider collider in capsules) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawWireCapsule(collider.center + transform.position, GetColliderDirection(collider.direction) * transform.rotation, collider.radius, collider.height, Color);
        }
#endif
    }

    private void OnDrawGizmosSelected() {
        if (AlwaysDraw) {
            return;
        }

        foreach (BoxCollider collider in boxes) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawBox(collider);
        }
        foreach (SphereCollider collider in spheres) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawSphere(collider);
        }
#if UNITY_EDITOR
        foreach (CapsuleCollider collider in capsules) {
            if (collider == null) {
                PopulateColliderArrays();
            }
            DrawWireCapsule(collider.center + transform.position, GetColliderDirection(collider.direction) * transform.rotation, collider.radius, collider.height, Color);
        }
#endif
    }

    [Button]
    void PopulateColliderArrays() {
        BoxCollider[] selfBoxes = GetComponents<BoxCollider>();
        BoxCollider[] childBoxes = GetComponentsInChildren<BoxCollider>();

        boxes = selfBoxes.Concat(childBoxes).ToArray();

        SphereCollider[] selfSpheres = GetComponents<SphereCollider>();
        SphereCollider[] childSpheres = GetComponentsInChildren<SphereCollider>();

        spheres = selfSpheres.Concat(childSpheres).ToArray();

        CapsuleCollider[] selfCapsules = GetComponents<CapsuleCollider>();
        CapsuleCollider[] childCapsules = GetComponentsInChildren<CapsuleCollider>();

        capsules = selfCapsules.Concat(childCapsules).ToArray();
    }

    void DrawBox(BoxCollider collider) {
        Gizmos.matrix = collider.transform.localToWorldMatrix;
        Gizmos.color = Color;
        if (DrawWire) {
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
        else {
            Gizmos.DrawCube(collider.center, collider.size);
        }
    }

    void DrawSphere(SphereCollider collider) {
        Gizmos.matrix = collider.transform.localToWorldMatrix;
        Gizmos.color = Color;
        if (DrawWire) {
            Gizmos.DrawWireSphere(collider.center, collider.radius);
        }
        else {
            Gizmos.DrawSphere(collider.center, collider.radius);
        }
    }

#if UNITY_EDITOR
    public Quaternion GetColliderDirection(int direction) {
        return direction switch {
            0 => Quaternion.Euler(0f, 0f, 90f),
            1 => Quaternion.Euler(0f, 90f, 0f),
            2 => Quaternion.Euler(90f, 0f, 0f),
            _=> Quaternion.Euler(90f, 0f, 0f)
        };
    }
    public void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color)) {
        if (_color != default(Color))
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix)) {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

        }
    }
#endif
}