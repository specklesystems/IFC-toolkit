using System.Drawing;
using System.Reflection;
using Ara3D.IfcLoader;
using Ara3D.IfcParser;
using Ara3D.StepParser;
using Objects.Geometry;
using Objects.Other;
using Speckle.Core.Models;

namespace Ara3D.Speckle.IfcLoader
{
    public static class IfcToSpeckle
    {
        public static Base ToSpeckle(this IfcFile f)
        {
            return f.ToSpeckle(f.Graph);
        }

        public static Base ToSpeckle(this IfcFile f, IfcGraph g)
        {
            var b = new Base();
            var children = g.GetSources().Select(f.ToSpeckle).ToList();
            b["elements"] = children;
            return b;
        }

        public static Base ToSpeckle(this IfcModel m)
        {
            var b = new Base();
            b["Name"] = "Root";
            return b;
        }

        public static unsafe Mesh ToSpeckle(this IfcMesh mesh)
        {
            var r = new Mesh();
            var vertexData = mesh.Vertices;
            var indexData = mesh.Indices;
            var m = (double*)mesh.Transform;
            var vp = (IfcVertex*)vertexData;
            var ip = (int*)indexData;

            for (var i = 0; i < mesh.NumVertices; i++)
            {
                var x = vp->PX;
                var y = vp->PY;
                var z = vp->PZ;
                r.vertices.Add(m[0] * x + m[4] * y + m[8] * z + m[12]);
                r.vertices.Add(-(m[2] * x + m[6] * y + m[10] * z + m[14]));
                r.vertices.Add(m[1] * x + m[5] * y + m[9] * z + m[13]);
            }

            for (var i = 0; i < mesh.NumIndices * 3; i += 3)
            {
                var a = ip[i];
                var b = ip[i + 1];
                var c = ip[i + 2];
                r.faces.Add(0);
                r.faces.Add(a);
                r.faces.Add(b);
                r.faces.Add(c);
            }

            var rm = new RenderMaterial();
            var color = (double*)mesh.Color;
            rm.diffuseColor = Color.FromArgb(
                (int)(color[0] * 255), 
                (int)(color[1] * 255), 
                (int)(color[2] * 255),
                (int)(color[3] * 255));
            r["renderMaterial"] = rm;
            return r;
        }

        public static Collection ToSpeckle(this IfcGeometry? geometry)
        {
            var c = new Collection();
            if (geometry != null)
                foreach (var tm in geometry.GetMeshes())
                    c.elements.Add(tm.ToSpeckle());
            return c;
        }

        public static object? IfcValJsonObject(this StepValue? sv)
        {
            switch (sv)
            {
                case StepEntity stepEntity:
                    return stepEntity.ToString();
                case StepId stepId:
                    return stepId.Id;
                case StepList stepList:
                    return stepList.Values.Select(IfcValJsonObject).ToList();
                case StepNumber stepNumber:
                    return stepNumber.AsNumber();
                case StepRedeclared stepRedeclared:
                    return null;
                case StepString stepString:
                    return stepString.AsString();
                case StepSymbol stepSymbol:
                    return stepSymbol.AsString();
                case StepUnassigned stepUnassigned:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sv));
            }
        }

        public static Dictionary<string, object> ToSpeckleDictionary(this IfcPropSet ps)
        {
            var d = new Dictionary<string, object>();
            foreach (var p in ps.GetProperties())
                d[p.Name] = p.Value.IfcValJsonObject();
            return d;
        }

        public static Base ToSpeckle(this IfcFile file, IfcNode n)
        {
            var b = new Base();
            if (n is IfcPropSet ps)
            {
                b["Name"] = ps.Name;
                b["GlobalId"] = ps.Guid;
            }

            // https://github.com/specklesystems/speckle-server/issues/1180
            b["ifc_type"] = n.Type;

            // This is required because "speckle_type" has no setter, but is backed by a private field.  
            var baseType = typeof(Base);
            var typeField = baseType.GetField("_type", BindingFlags.Instance | BindingFlags.NonPublic);
            typeField?.SetValue(b, n.Type);

            // Guid is null for property values, and other Ifc entities not derived from IfcRoot 
            b.applicationId = n.Guid;

            // This is the express ID used to identify an entity wihtin a file.
            b["expressID"] = n.Id;
            
            // Even if there is no geometry, this will return an empty collection. 
            var c = file.Model.GetGeometry(n.Id).ToSpeckle();
            if (c.elements.Count > 0)
                b["displayValue"] = c.elements;

            // Create the children 
            var children = n.GetChildren().Select(file.ToSpeckle).ToList();
            b["elements"] = children;

            // Add the properties
            foreach (var p in n.GetPropSets())
                b[p.Name] = p.ToSpeckleDictionary();

            return b;
        }
    }
}