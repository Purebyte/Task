using UnityEngine;
using UnityEngine.U2D;

public class TerrainGenerator : MonoBehaviour
{
    [HideInInspector]
    public int generateCount = 0;
    [HideInInspector]
    public int multiplier = 0;
    [HideInInspector]
    public int multiplierx2 = 90;
    [HideInInspector]
    public SpriteShapeController shape;

    [Header("ƒлина сплайна")]
    public int scalePosition = 1000;
    [Header(" оличество генерируемых точек на сплайне")]
    public int numOfPoints = 100;
    [Header("¬ысота точек сплайна по y")]
    public int scaleHeight = 10;

    public void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        float distanceBtwnPoints = scalePosition / numOfPoints; // рассто€ние между точками по x
        shape.spline.SetPosition(2, shape.spline.GetPosition(2) + Vector3.right * scalePosition); //ѕеремещаем вторую и третью точку
        shape.spline.SetPosition(3, shape.spline.GetPosition(3) + Vector3.right * scalePosition); //—плайна вправо

        for (int i = 0; i < 90; i++)
        {
            float xPos = shape.spline.GetPosition(i + 1).x + distanceBtwnPoints; //ѕолучаем позицию точек по x, начина€ с первой
            shape.spline.InsertPointAt(i + 2, new Vector3(xPos, scaleHeight * Mathf.PerlinNoise
                (i * Random.Range(25.0f, 75.0f), 0)));
        }
    
        for (int i = 2; i < 92; i++)
        {
            shape.spline.SetPosition(91, new Vector3(shape.spline.GetPosition(91).x, 2, 0)); //обрезаем высоту конца участка дл€ чекпоинта
            shape.spline.SetPosition(92, new Vector3(shape.spline.GetPosition(92).x, 2, 0));
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous); //”станавливаем режим касательной
            shape.spline.SetLeftTangent(i, new Vector3(-1.0f, 0, 0)); // орректируем длину левой и правой касательных
            shape.spline.SetRightTangent(i, new Vector3(1.0f, 0f, 0));
        }
    }
    public void GenerateNextSpline()
    {
        ResetSpline();
        multiplier += 90;
        multiplierx2 += 90;
        generateCount++;
        scalePosition = 1000 * generateCount;
        numOfPoints = 100 * generateCount;
        float distanceBtwnPoints = scalePosition / numOfPoints; // рассто€ние между точками по x
        shape.spline.SetPosition(2 + multiplier, shape.spline.GetPosition(2 + multiplier) + Vector3.right * scalePosition); //ѕеремещаем вторую и третью точку
        shape.spline.SetPosition(3 + multiplier, shape.spline.GetPosition(3 + multiplier) + Vector3.right * scalePosition); //—плайна вправо


        for (int i = 1 * multiplier; i < 1 * multiplierx2; i++) //c 90й до 180й
        {
            float xPos = shape.spline.GetPosition(i + 1).x + distanceBtwnPoints; //ѕолучаем позицию точек по x, начина€ с первой
            shape.spline.InsertPointAt(i + 2, new Vector3(xPos, scaleHeight * Mathf.PerlinNoise
                (i * Random.Range(25.0f, 75.0f), 0))); //ƒобавл€ем точки сплайна
        }

       for (int i = 2 + multiplier; i < 2 + multiplierx2; i++) //с 92й до 182й
        {
            shape.spline.SetPosition(1 + multiplierx2, new Vector3(shape.spline.GetPosition(1 + multiplierx2).x, 2, 0)); //обрезаем высоту конца участка при генерации
            shape.spline.SetPosition(2 + multiplierx2, new Vector3(shape.spline.GetPosition(2 + multiplierx2).x, 2, 0));
            shape.spline.SetPosition(2 + multiplier, new Vector3(shape.spline.GetPosition(2 + multiplier).x, 2, 0)); //обрезаем высоту начала участка чтобы не "застр€ть в текстурах"
            shape.spline.SetPosition(3 + multiplier, new Vector3(shape.spline.GetPosition(3 + multiplier).x, 2, 0));
            shape.spline.SetPosition(4 + multiplier, new Vector3(shape.spline.GetPosition(4 + multiplier).x, 2, 0));
            shape.spline.SetPosition(5 + multiplier, new Vector3(shape.spline.GetPosition(5 + multiplier).x, 2, 0));
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous); //”станавливаем режим касательной
            shape.spline.SetLeftTangent(i, new Vector3(-1.2f, 0, 0)); // орректируем длину левой и правой касательных
            shape.spline.SetRightTangent(i, new Vector3(1.2f, 0f, 0));
        }
    }
    public void DeletePreviousSpline()
    {
        for (int i = 2; i < 90; i++)
        {
            //shape.spline.RemovePointAt(i); //Ќе работает как должно
            //Debug.Log(i);
        }
    }

    public void ResetSpline()
    {
        scalePosition = 0;
        numOfPoints = 0;
        generateCount = 0;
    }
}