//Shady
using UnityEngine;
using System.Collections.Generic;

namespace Shady
{
    public class Draw : MonoBehaviour
    {
        [SerializeField] Camera Cam = null;
        [SerializeField] LineRenderer trailPrefab = null;

        private LineRenderer currentTrail;
        private List<Vector3> points = new List<Vector3>();

        public enum DrawMode
        {
            Drawing,
            Erasing
        }

        private DrawMode currentMode = DrawMode.Drawing;

        void Start()
        {
            if (!Cam)
            {
                Cam = Camera.main;
            }
        }

        void Update()
        {
            // Switch between modes
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentMode == DrawMode.Drawing)
                    currentMode = DrawMode.Erasing;
                else
                    currentMode = DrawMode.Drawing;

                Debug.Log("Mode actuel : " + currentMode);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (currentMode == DrawMode.Drawing)
                {
                    CreateNewLine();
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (currentMode == DrawMode.Drawing)
                {
                    AddPoint();
                }
                else if (currentMode == DrawMode.Erasing)
                {
                    ErasePoint();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (transform.childCount != 0)
                {
                    foreach (Transform R in transform)
                    {
                        Destroy(R.gameObject);
                    }
                }
            }
        }

        private void CreateNewLine()
        {
            currentTrail = Instantiate(trailPrefab);
            currentTrail.transform.SetParent(transform, true);
            currentTrail.useWorldSpace = false; // Désactiver l'utilisation de l'espace monde
            points.Clear();
        }

        private void UpdateLinePoints()
        {
            if (currentTrail != null && points.Count > 0)
            {
                currentTrail.positionCount = points.Count;
                currentTrail.SetPositions(points.ToArray());
            }
        }

        private void AddPoint()
        {
            var Ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                if (hit.collider.CompareTag("Writeable"))
                {
                    // Convertir la position du point en espace local
                    Vector3 localPoint = currentTrail.transform.InverseTransformPoint(hit.point);
                    points.Add(localPoint);
                    UpdateLinePoints();
                }
            }
        }

        private void ErasePoint()
        {
            var Ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                if (hit.collider.CompareTag("Writeable"))
                {
                    // Parcourir tous les LineRenderers enfants pour trouver et effacer les points proches
                    foreach (Transform child in transform)
                    {
                        LineRenderer line = child.GetComponent<LineRenderer>();
                        if (line != null)
                        {
                            ErasePointsInLine(line, hit.point);
                        }
                    }
                }
            }
        }

        private void ErasePointsInLine(LineRenderer line, Vector3 hitPoint)
        {
            // Convertir le point de collision en espace local du LineRenderer
            Vector3 localHitPoint = line.transform.InverseTransformPoint(hitPoint);

            int numPositions = line.positionCount;
            Vector3[] linePoints = new Vector3[numPositions];
            line.GetPositions(linePoints);

            float eraseRadius = 0.1f; // Rayon de la gomme

            List<Vector3> remainingPoints = new List<Vector3>();

            // Trouver les points à conserver
            for (int i = 0; i < linePoints.Length; i++)
            {
                if (Vector3.Distance(linePoints[i], localHitPoint) > eraseRadius)
                {
                    remainingPoints.Add(linePoints[i]);
                }
            }

            // Mettre à jour le LineRenderer
            if (remainingPoints.Count > 1)
            {
                line.positionCount = remainingPoints.Count;
                line.SetPositions(remainingPoints.ToArray());
            }
            else
            {
                // Si le LineRenderer n'a plus assez de points, le détruire
                Destroy(line.gameObject);
            }
        }
    }
}
