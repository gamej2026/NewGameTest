using UnityEngine;

namespace Honbul
{
    public static class HanokAlleyBuilder
    {
        public static Material UrpLit(Color color, bool emission)
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.SetColor("_BaseColor", color);

            if (emission)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 2f);
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }

            return material;
        }

        public static GameObject Build()
        {
            GameObject root = new GameObject("Environment");

            Material groundMat = UrpLit(new Color(0.2f, 0.2f, 0.21f), false);
            Material wallMat = UrpLit(new Color(0.27f, 0.24f, 0.22f), false);
            Material roofMat = UrpLit(new Color(0.16f, 0.16f, 0.17f), false);
            Material propMat = UrpLit(new Color(0.3f, 0.27f, 0.24f), false);

            CreatePrimitive(PrimitiveType.Plane, "Ground", root.transform, new Vector3(0f, 0f, 14f), new Vector3(2.6f, 1f, 2.8f), groundMat);

            CreatePrimitive(PrimitiveType.Cube, "LeftWall_0", root.transform, new Vector3(-3f, 1.5f, 4f), new Vector3(1f, 3f, 8f), wallMat);
            CreatePrimitive(PrimitiveType.Cube, "LeftWall_1", root.transform, new Vector3(-3f, 1.5f, 13f), new Vector3(1f, 3f, 8f), wallMat);
            CreatePrimitive(PrimitiveType.Cube, "LeftWall_2", root.transform, new Vector3(-3f, 1.5f, 22f), new Vector3(1f, 3f, 8f), wallMat);

            CreatePrimitive(PrimitiveType.Cube, "RightWall_0", root.transform, new Vector3(3f, 1.5f, 6f), new Vector3(1f, 3f, 7f), wallMat);
            CreatePrimitive(PrimitiveType.Cube, "RightWall_1", root.transform, new Vector3(3f, 1.5f, 15f), new Vector3(1f, 3f, 7f), wallMat);
            CreatePrimitive(PrimitiveType.Cube, "RightWall_2", root.transform, new Vector3(3f, 1.5f, 24f), new Vector3(1f, 3f, 7f), wallMat);

            CreatePrimitive(PrimitiveType.Cube, "RoofEave_Left", root.transform, new Vector3(-2.4f, 3.2f, 14f), new Vector3(0.6f, 0.4f, 24f), roofMat);
            CreatePrimitive(PrimitiveType.Cube, "RoofEave_Right", root.transform, new Vector3(2.4f, 3.2f, 14f), new Vector3(0.6f, 0.4f, 24f), roofMat);

            CreatePrimitive(PrimitiveType.Cylinder, "Jar_0", root.transform, new Vector3(-1.8f, 0.45f, 9f), new Vector3(0.6f, 0.45f, 0.6f), propMat);
            CreatePrimitive(PrimitiveType.Cylinder, "Jar_1", root.transform, new Vector3(1.7f, 0.45f, 18f), new Vector3(0.55f, 0.45f, 0.55f), propMat);

            CreatePrimitive(PrimitiveType.Cylinder, "Pillar_0", root.transform, new Vector3(-2.2f, 1.5f, 12f), new Vector3(0.25f, 1.5f, 0.25f), wallMat);
            CreatePrimitive(PrimitiveType.Cylinder, "Pillar_1", root.transform, new Vector3(2.2f, 1.5f, 20f), new Vector3(0.25f, 1.5f, 0.25f), wallMat);

            return root;
        }

        private static GameObject CreatePrimitive(
            PrimitiveType type,
            string name,
            Transform parent,
            Vector3 position,
            Vector3 scale,
            Material material)
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.name = name;
            obj.transform.SetParent(parent, false);
            obj.transform.position = position;
            obj.transform.localScale = scale;

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }

            return obj;
        }
    }
}