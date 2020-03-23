using System;

class Program
{
    //Funkcja zamieniajaca miejscami 2 elemty
    public static void Swap<T>(ref T left, ref T right) 
    {
        T temp;
        temp = left;
        left = right;
        right = temp;
    }

    //Funkcja porwónująca elementy
    private static bool Increasing <T>( T item1, T item2) where T: IComparable<T>
    {
        return item1.CompareTo(item2)<0;
    } 

    //Funkcja porwónująca elementy
    private static bool Decreasing <T>( T item1,  T item2) where T: IComparable<T>
    {
        return !Increasing(item1, item2);
    }

    //Funkcja dzielaca na podzbiory jako piwot wybierana jest wartość środkowa przedzialu [left,right]
    private static int Partition<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        var pivotValue= array[(left+right)/2];//Piwot jest wybierany jako środek dzielonej tablicy;
        Swap(ref array[(left+right)/2],ref array[right]); //przeniesienie piwotu na koniec aby nie brał udziału w sortowaniu;

        int pivotPosition = left;
        while(array[pivotPosition].CompareTo(pivotValue)<0 && pivotPosition.CompareTo(right)<0) pivotPosition++;// szuka wartości większej od pivotu

        int i=pivotPosition;//zaczyna sortowanie od pozycji nie posortowanej
        while(i<right)
        {
            if(array[i].CompareTo(pivotValue)<0)
            {
                Swap(ref array[i],ref array[pivotPosition]);
                ++pivotPosition;
            } 
            i++;
        }

        Swap(ref array[right],ref array[pivotPosition]); //powrót piwotu na pozycje dzieląca tablice
        return pivotPosition;
    } 

//Sortowanie szybkie
    private static void Quicksort<T>(T[] array, int left, int right) where T: IComparable<T>
    {
        if(left>=right) return;

        int pivotPosition = Partition<T>(array, left, right);//Podział tablicy na część i zwraca pozycję piwotu;
        Quicksort(array, left, pivotPosition - 1); //Sortowanie tablicy na lewo od piwotu;
        Quicksort(array, pivotPosition + 1, right);//Sortowanie na prawo od piwotu;
    }

  //Sortowanie przez scalanie funkcja laczaca tablice
    private static void Merge<T>(T[] array, int left,int middle, int right) where T:IComparable<T>
    {
        T[] tempArr = new T [middle - left]; //tablica pomocnicza;

        for(int i = left; i < middle; i++)
            tempArr[i-left] = array[i]; //przekopiowywanie do pomocniczej;

        int iArr = left, iLeft = 0, iRight = middle;

        while((iLeft+left) < middle && iRight <= right){ //przekopiowuje do własciwej wartosc mniejsza
            if(Increasing( tempArr[iLeft], array[iRight]) )// z lewej lub z prawej tablicy 
                array[iArr++] = tempArr[iLeft++];
            else
                array[iArr++] = array[iRight++];
        }

        for (int i = iLeft; (i + left) < middle; i++)
            array[iArr++] = tempArr[i]; //przepisujemy pozostałe elementy
        

    }



//Sortowanie przez scalanie
    private static void MergeSort<T>(T[] array, int left, int right) where T: IComparable<T>
    {
        if(right<=left) return;
        
        int middle = (left+right)/2; //srodek tablicy
        MergeSort(array, left, middle); //dzielenie tablicy na lewo od srodka;
        MergeSort(array, middle+1 , right);//dzielenie tablicy na prawo od sroda;
        Merge(array, left, middle+1, right); // scalanie podzielonych tablic
        
    }

  //Sortowanie przez scalanie z możliwością wyboru malejaco/rosnaco
    private static void Merge<T>(T[] array, int left,int middle, int right,Func<T,T,bool> TypeSort) where T:IComparable<T>
    {
        T[] tempArr = new T [middle - left]; //tablica pomocnicza;

        for(int i = left; i < middle; i++)
            tempArr[i-left] = array[i]; //przekopiowywanie do pomocniczej;

        int iArr = left, iLeft = 0, iRight = middle;

        while((iLeft+left) < middle && iRight <= right){ //przekopiowuje do własciwej wartosc mniejsza
            if(TypeSort( tempArr[iLeft], array[iRight]) )// z lewej lub z prawej tablicy 
                array[iArr++] = tempArr[iLeft++];
            else
                array[iArr++] = array[iRight++];
        }

        for (int i = iLeft; (i + left) < middle; i++)
            array[iArr++] = tempArr[i]; //przepisujemy pozostałe elementy
        

    }

    //Sortowanie przez scalanie z możliwością wyboru malejaco/rosnaco
    private static void MergeSort<T>(T[] array, int left, int right, Func<T,T,bool> TypeSort) where T: IComparable<T>
    {
        if(right<=left) return;
        
        int middle = (left+right)/2; //srodek tablicy
        MergeSort(array, left, middle,TypeSort); //dzielenie tablicy na lewo od srodka;
        MergeSort(array, middle+1 , right, TypeSort);//dzielenie tablicy na prawo od sroda;
        Merge(array, left, middle+1, right, TypeSort); // scalanie podzielonych tablic
    }

    //Funkcja tworzaca kopiec
    //startArrayIndex to miejsce które uznawane jest za poczatek "tablicy",
    //Wykorzystywane w sortowaniu fragmentów tablic np elementy [10-20]
    private static void Heapify<T>( T[] array, int startIndex, int endIndex, int startArrayIndex) where T: IComparable<T>
    {
        int leftBaby = (startIndex + 1) * 2 - 1 - startArrayIndex;
        int rightBaby = (startIndex + 1) * 2 - startArrayIndex;
        int largest = startIndex;

        //Sprawdzanie czy dzieci nie sa wieksze od korzenia
        if(leftBaby <= endIndex && array[leftBaby].CompareTo(array[largest])>0)
        {
            largest=leftBaby;
        }

        if(rightBaby <= endIndex && array[rightBaby].CompareTo(array[largest])>0)
        {
            largest=rightBaby;
        }

        if(largest!=startIndex)
        {
            Swap(ref array[startIndex],ref array[largest]);//na pozycje korzenia wskakuje najwieksza wartosc

            Heapify( array, largest, endIndex, startArrayIndex);//Tworz dalsza czesc kopca
        }
    }


    //Sortowanie przez kopcowanie
    private static void HeapSort<T>(T[] array, int left, int right) where T: IComparable<T>
    {
        int length = right-left;

        //Tworzenie kopca
        for(int i = length / 2 + left; i >= left; --i)
        {
            Heapify( array, i, right, left);
        }
        //rozpakowywanie kopca element po elemencie
        for(int i = right ; i > left; --i)
        {
            Swap(ref array[left], ref array[i]);//przenosi korzen(max) na koniec
            --right;
            Heapify( array, left, right, left); // Przekształcenie reszty w strukture kopca wraz z jego zasadami
        }
    }

    //Funkcja IntroSort hybrydowa wykorzystujaca sortowanie kopcowe oraz sortowanie szybkie
    //depthLimit jest to głebokość wywołań rekurencyjnych przy którym wowała się HeapSort
    private static void IntroSort<T>(T[] array, int left, int right, int depthLimit) where T: IComparable<T>
    {
        if(depthLimit == 0)
        {
            HeapSort(array, left, right);
            return;
        }
        else
        {
        if(left>=right) return;
        int pivotPosition = Partition<T>(array, left, right);//Podział tablicy na część i zwraca pozycję piwotu;
        IntroSort(array, left, pivotPosition - 1, depthLimit - 1); //Sortowanie tablicy na lewo od piwotu;
        IntroSort(array, pivotPosition + 1, right, depthLimit - 1);//Sortowanie na prawo od piwotu;
        }

    }


    private static void IntroSort<T>(T[] array, int left, int right ) where T: IComparable<T>
    {
        int depthLimit =(int) Math.Floor( 2 * Math.Log(array.Length)/Math.Log(2));
        IntroSort(array, left, right, depthLimit);
    }

    //Funkcja losujaca elementy tablicy array
    //seed umożliwia tworzenie powtarzalnych danych do tablic
    //offset umożliwia róznych wartości dla kazdej z kolejnych tablic
    //splitPoint Wyznacza punkt do którego losuja się elementy z zakresu <0,max)
    // od splitPoint (włącznie) elementu tablicy losują sie z zakresu <max,2max)

    private static void Draw( int[] array, int offset = 0, int splitPoint=0, int max=100000)
    {
        int seed = 12 + offset;
        Random rnd = new Random(seed);

        for (int i = 0; i < splitPoint; i++)
        {
         array[i] = rnd.Next()%max;
        }

        for (int i = splitPoint; i < array.Length; i++)
        {
         array[i] = rnd.Next()%max + max;
        }

    }

//Funkcja tworzaca tablice postrzepiona
    private static int[][] JaggedArray(int rows, int cows)
    {   
        int[][] jaggArray = new int [rows][];
        for (int i = 0; i < rows; i++)
        {
            jaggArray[i]= new int [cows];
        }

        return jaggArray;
    }

//Funkcja wypelniajac tablice postrzepione 
//splitPoint Wyznacza punkt od którego losuja się elementy większe
// od maksymalnego elementu tablicy do tego punktu
    private static void FillJagged( int[][] array, int splitPoint=0)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Draw(array[i], i,splitPoint);
        }
    }

//Funkcja wypisujaca tablice jednowymiarowe
    private static void Display(int[] array)
    {
        foreach(var element in array)
        {
            Console.Write(element + " ");
        }
        Console.WriteLine("");
    }

//funkcja wypisująca tablice postrzepione
    private static void Display(int[][] jaggArray)
    {
        for(int i = 0; i < jaggArray.Length; i++)
        {
            Display(jaggArray[i]);
        }
    }

//Funcja sprawdzająca czy otrzymanne tablice są identyczne
    private static bool Check(int[][] array1, int[][] array2)
    {
        for(int i = 0; i < array1.Length; i++)
        {
           for(int x = 0; x < array1[i].Length; x++)
            {
                if(array1[i][x]!=array2[i][x])
                {
                   return false;
                }
            } 
        }
        return true;
    }
//funkcja sortująca tablice dla wybranej metody sortującej: QuicSort/IntroSort/MergeSort
//oraz mierzaca czas sortowania.
  private static void Sort<T>(T[][] jaggArray, Action<T[] , int, int> sortMetod) where T: IComparable<T>
    {   System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        for (int i = 0; i < jaggArray.Length; i++)
        {
            sortMetod( jaggArray[i], 0, jaggArray[i].Length-1);
        }
    
        watch.Stop();
        Console.WriteLine("czas wykonania w ms: " + watch.ElapsedMilliseconds);
    }

//Funkcja sortująca n elementów w tablicy.
    private static void Sort<T>(T[][] jaggArray, Action<T[] , int, int> sortMetod, int n) where T: IComparable<T>
    {   
        for (int i = 0; i < jaggArray.Length; i++)
        {
            sortMetod( jaggArray[i], 0, n-1);
        }
    }

//Funkcja do posortowania według wybranego typu, możliwość napisana tylko dla MergeSort.
    private static void Sort<T>(T[][] jaggArray, Func<T,T,bool> TypeSort) where T: IComparable<T>
    {
         for (int i = 0; i < jaggArray.Length; i++)
        {
            MergeSort(jaggArray[i],0,jaggArray[i].Length-1,TypeSort);
        }
    }

    static void Main(string[] args)
    {
        Console.Write("Wprowadź liczbe elementów: ");
        int n =int.Parse(Console.ReadLine());
        int k = 0;
      
         Console.WriteLine("Merge Sort!");
         int[][] arrayMerge = JaggedArray(100, n);
         FillJagged(arrayMerge,k);
        // //Display(array);
       // Sort(arrayMerge,MergeSort,k);
        Sort(arrayMerge,Decreasing);
        Sort(arrayMerge,MergeSort);
        // Sort(arrayMerge,Decreasing);
      
       // Display(arrayMerge[0]);
        Console.WriteLine("Quick Sort!");

        int[][] arrayQuick = JaggedArray(100, n);
        FillJagged(arrayQuick,k);
       // Quicksort(arrayQuick[0], ,k);
        //Sort(arrayQuick,Quicksort,k);
        Sort(arrayQuick,Decreasing);
        Sort(arrayQuick,Quicksort);

        Console.WriteLine("Intro Sort!");
        int[][] arrayIntro = JaggedArray(100, n);
        FillJagged(arrayIntro,k);
       // HeapSort(arrayIntro[0], s,k-1);
        //Sort(arrayIntro,Quicksort,k);
        Sort(arrayIntro,Decreasing);
        Sort(arrayIntro,IntroSort);
      //  Display(arrayIntro);
        Console.WriteLine(Check(arrayMerge, arrayQuick));
        Console.WriteLine(Check(arrayIntro, arrayQuick));
    }
}
