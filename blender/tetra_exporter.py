# TETRA Exporter Addon for Blender
# Part of the TETRA project: https://github.com/marcobuttiglione/tetra
# Author: Marco Domenico Buttiglione
# License: MIT
#
# This exporter works in tandem with the Blender tetrahedralization plugin by Matthias Mueller
# (https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/BlenderTetPlugin.py) to export tetrahedral meshes into a custom .tetra format.
#
# The .tetra format is a simple text-based format containing vertices and tetrahedra (4 indices per element).

bl_info = {
    "name": "Export Tetrahedral Mesh (.tetra)",
    "author": "Marco Domenico Buttiglione",
    "version": (1, 0),
    "blender": (2, 93, 0),
    "location": "File > Export > Tetrahedral Mesh (.tetra)",
    "description": "Export a tetrahedral mesh to a custom .tetra format",
    "category": "Import-Export",
}

import bpy
from bpy_extras.io_utils import ExportHelper, axis_conversion
from bpy.props import StringProperty, EnumProperty
from bpy.types import Operator


class ExportTetrahedralMesh(Operator, ExportHelper):
    """Export mesh as tetrahedral .tetra format"""
    bl_idname = "export_mesh.tetrahedral_tetra"
    bl_label = "Export Tetrahedral Mesh (.tetra)"
    bl_options = {'PRESET'}

    filename_ext = ".tetra"
    filter_glob: StringProperty(default="*.tetra", options={'HIDDEN'})

    # Axis options to match the coordinate systems (e.g. Blender ↔ Unity)
    axis_forward: EnumProperty(
        name="Forward Axis",
        items=[
            ('X', "X Forward", ""),
            ('Y', "Y Forward", ""),
            ('Z', "Z Forward", ""),
            ('-X', "-X Forward", ""),
            ('-Y', "-Y Forward", ""),
            ('-Z', "-Z Forward", ""),
        ],
        default='-Z'
    )

    axis_up: EnumProperty(
        name="Up Axis",
        items=[
            ('X', "X Up", ""),
            ('Y', "Y Up", ""),
            ('Z', "Z Up", ""),
            ('-X', "-X Up", ""),
            ('-Y', "-Y Up", ""),
            ('-Z', "-Z Up", ""),
        ],
        default='Y'
    )

    def draw(self, context):
        layout = self.layout
        layout.prop(self, "axis_forward")
        layout.prop(self, "axis_up")

    def execute(self, context):
        obj = context.active_object
        if not obj or obj.type != 'MESH':
            self.report({'ERROR'}, "Active object is not a mesh")
            return {'CANCELLED'}

        mesh = obj.data
        mesh.calc_loop_triangles()

        # Convert to selected coordinate system
        global_matrix = axis_conversion(to_forward=self.axis_forward, to_up=self.axis_up).to_4x4()

        with open(self.filepath, 'w') as f:
            f.write(f"# .tetra file exported from Blender object: {obj.name}\n")
            f.write(f"# Vertices:\n")

            # Write vertices
            for v in mesh.vertices:
                co = global_matrix @ v.co
                f.write(f"v {co.x} {co.y} {co.z}\n")

            f.write(f"# Tetrahedra:\n")

            # Write tetrahedral elements from quad faces
            for poly in mesh.polygons:
                if len(poly.vertices) == 4:
                    indices = [i for i in poly.vertices]
                    f.write(f"t {indices[0]} {indices[1]} {indices[2]} {indices[3]}\n")
                else:
                    self.report({'WARNING'}, f"Skipped non-quad face with {len(poly.vertices)} vertices")

        self.report({'INFO'}, f"Exported to {self.filepath}")
        return {'FINISHED'}


def menu_func_export(self, context):
    self.layout.operator(ExportTetrahedralMesh.bl_idname, text="Tetrahedral Mesh (.tetra)")


def register():
    bpy.utils.register_class(ExportTetrahedralMesh)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export)


def unregister():
    bpy.utils.unregister_class(ExportTetrahedralMesh)
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export)


if __name__ == "__main__":
    register()
