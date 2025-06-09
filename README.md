# TETRA

**TETRA** is an open-source pipeline for exporting and importing volumetric tetrahedral meshes from Blender to Unity (or other engines), built on top of the tetrahedralization plugin by Matthias Müller – Ten Minute Physics.

This repository provides:

* A tetrahedralization plugin for Blender
* A `.tetra` file format exporter
* A Unity-compatible format and importer for use in simulations or visualization
* Format documentation and example meshes

---

## Features

* Generate volumetric tetrahedral meshes directly in Blender
* Export to `.tetra`: a clean, human-readable format
* Coordinate system conversion (Blender → Unity)
* Unity importer to load `.tetra` files as assets
* Easy integration with simulations, rendering, or analysis

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/marcobuttiglione/tetra.git
```

---

### 2. Install Blender Add-ons

In Blender:

1. Open **Edit > Preferences > Add-ons**
2. Click **Install...**
3. Select the following files from the `blender/` folder:

   * `BlenderTetPlugin.py` (tetrahedral mesh generation)
   * `tetra_exporter.py` (exports to `.tetra` format)
4. Enable both add-ons from the list

---

### 3. Create and Export a Tetrahedral Mesh

1. Select a mesh in Object Mode
2. Use **Shift + A > Mesh > Add Tetrahedralization**
3. Adjust resolution and quality parameters, then confirm
4. With the resulting mesh selected, go to:

   ```
   File > Export > Tetrahedral Mesh (.tetra)
   ```
5. Choose a destination and export

---

## Unity Integration

The Unity importer (`TetraImporter.cs`) reads `.tetra` files and converts them into `ScriptableObject` assets of type `TetrahedralMeshAsset`, which contain:

* Vertex coordinates
* Tetrahedra vertex indices
* Automatically extracted edge indices

These can be used for simulations, visualizations, or custom mesh processing.

---

## `.tetra` File Format

The `.tetra` format is a simple text-based format with two main elements:

```
v x y z     # defines a vertex

t i j k l   # defines a tetrahedron by 0-based vertex indices
```

### Example

```txt
# Vertices
v 0.0 0.0 0.0
v 1.0 0.0 0.0
v 0.0 1.0 0.0
v 0.0 0.0 1.0

# Tetrahedra
t 0 1 2 3
```

See [`docs/format_spec.md`](./docs/format_spec.md) for full specification.

---

## Repository Structure

```
/blender/
  BlenderTetPlugin.py       - Tetrahedral mesh generation
  tetra_exporter.py         - .tetra format exporter

/docs/
  format_spec.md            - File format documentation

/examples/
  test_mesh.tetra           - Example .tetra mesh file

/unity/
  TetraImporter.cs          - Unity asset importer
  TetrahedralMeshAsset.cs   - ScriptableObject asset structure

README.md
LICENSE
```

---

## Credits

This project is based on the work of:

**Matthias Müller – Ten Minute Physics**
YouTube: [Ten Minute Physics](https://www.youtube.com/@TenMinutePhysics)
Original plugin: [BlenderTetPlugin.py](https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/BlenderTetPlugin.py)

Included under the MIT License.

---

## License

This project is released under the MIT License.
See the [`LICENSE`](./LICENSE) file for details.

---

## Contributing

Contributions are welcome! Feel free to open issues or pull requests to:

* Improve Blender or Unity integration
* Extend the `.tetra` format
* Add support for metadata, regions, or visualization tools
