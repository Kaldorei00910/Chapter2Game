namespace Chapter2Game
{
    internal class Program
    {
        static void Main(string[] args)
        {

            char answer = ShowMainScreen();

            if (answer == '1')
            {
                Console.WriteLine("상태 보기 실행");
                //상태보기 
            }
            else if (answer == '2')
            {
                Console.WriteLine("인벤토리 실행");
                //인벤토리 
            }
            else if (answer == '3')
            {
                Console.WriteLine("상점 실행");
                //상점
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                //메인화면으로
                answer = ShowMainScreen();
            }

        }

        private static char ShowMainScreen()
        {
            char answer = 'a';
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다");
            Console.WriteLine("\n1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            answer = Char.Parse(Console.ReadLine());
            return answer;
        }
    
    
    }
}
