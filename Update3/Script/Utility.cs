using System.Collections;


public class Utility
{
    public static T[] ShuffleArray<T>(T[] array, int seed){
        System.Random prng = new System.Random (seed);

        // -1 = 마지막 배열은 생략해도 되기 때문
        for(int i = 0; i < array.Length-1; i++){
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
}
