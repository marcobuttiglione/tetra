# TETRA File Format Specification

The `.tetra` file format is a simple, human-readable format designed to store **volumetric tetrahedral meshes**. It contains vertex positions and tetrahedral elements defined by four vertex indices.

---

## 1. Structure

Each line of a `.tetra` file represents either:

- A **vertex position**, using the `v` prefix
- A **tetrahedral element**, using the `t` prefix

Comments can be included using `#`.

---

### Vertex Definition

Each vertex line starts with `v` followed by the X, Y, Z coordinates.

```txt
v 1.0 0.0 0.0
v 0.0 1.0 0.0
v 0.0 0.0 1.0
````

---

### Tetrahedron Definition

Each tetrahedron line starts with `t` followed by four **0-based** vertex indices.

```txt
t 0 1 2 3
```

This defines a tetrahedron using the 4 vertices with indices 0, 1, 2, and 3 (in the order defined in the vertex list).

---

## 2. Example

A simple `.tetra` file representing a single tetrahedron might look like this:

```txt
# Vertices
v 0.0 0.0 0.0
v 1.0 0.0 0.0
v 0.0 1.0 0.0
v 0.0 0.0 1.0

# Tetrahedra
t 0 1 2 3
```

---

## 3. Notes

* Indices are **zero-based**
* All tetrahedra should reference valid vertices
* No assumptions are made about orientation, winding, or manifoldness
* This format does **not** support metadata, normals, colors, or surface extraction — it is designed for simplicity

---

## 4. Compatibility

This format is intended to be used with:

* The **TETRA Exporter** for Blender
* The **TETRA Importer** for Unity or other custom engines
* Future tools that may extend `.tetra` with optional headers or regions

---

## 5. Licensing

This specification and format are provided under the MIT License, as part of the [TETRA project](https://github.com/marcobuttiglione/tetra).

