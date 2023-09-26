using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public float carbohydrates;
    public float protein;
    public float fat;
    public float sugar;
    public float dietaryFiber;
    public float vitamins;
}

public class FoodManager : MonoBehaviour
{
    public NutrientController nutrientController;

    // 음식 충돌 이벤트 처리
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            // FoodObject 스크립트에서 영양소 값을 가져옴
            FoodObject food = collision.gameObject.GetComponent<FoodObject>();

            // 가져온 영양소 값을 NutrientController로 전달하여 업데이트
            nutrientController.UpdateNutrients(food.carbohydrates, food.protein, food.fat, food.sugar, food.dietaryFiber, food.vitamins);

            // 음식 오브젝트를 제거
            Destroy(collision.gameObject);
        }
    }
}

