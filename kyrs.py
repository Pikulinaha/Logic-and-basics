import random


class KuhnAlgorithm:
    def __init__(self):
        self.size = 0
        self.graph = []
        self.n = []
        self.mt = []
        self.used = []
        self.gen_create = False
        self.gen_init = False

    def create_matrix(self, size):
        """Создание матрицы"""
        self.gen_create = True
        self.size = size
        self.n = [[0] * size for _ in range(size)]
        return self.n

    def initialize_random(self):
        """Заполнение матрицы 0 или 1"""
        if not self.gen_create:
            return False

        self.gen_init = True
        for i in range(self.size):
            for j in range(self.size):
                self.n[i][j] = random.randint(0, 1)
        return True

    def initialize_manual(self):
        """Ручной ввод"""
        if not self.gen_create:
            return False

        self.gen_init = True
        print(f"Введите граф размером {self.size}×{self.size} (по строкам, только 0 и 1):")
        for i in range(self.size):
            row = input(f"Строка {i + 1} ({self.size} чисел через пробел): ").split()
            if len(row) != self.size:
                print(f"Ошибка: нужно ввести ровно {self.size} чисел")
                return False
            for j in range(self.size):
                self.n[i][j] = int(row[j])
        return True

    def print_matrix(self):
        """Вывод матрицы"""
        if not (self.gen_create and self.gen_init):
            return False

        print("\nМатрица смежности графа:")
        for i in range(self.size):
            for j in range(self.size):
                print(f"{self.n[i][j]:3}", end="")
            print()
        return True

    def generate_adjacency_list(self):
        """Генерация смежности"""
        self.graph = [[] for _ in range(self.size)]
        for i in range(self.size):
            for j in range(self.size):
                if self.n[i][j] != 0:
                    self.graph[i].append(j)

    def khun(self, v):
        """Рекурсивная часть алгоритма Куна"""
        if self.used[v]:
            return False

        self.used[v] = True
        for to in self.graph[v]:
            if self.mt[to] == -1 or self.khun(self.mt[to]):
                self.mt[to] = v
                return True
        return False

    def find_max_matching(self):
        """Поиск наибольшего паросочетания"""
        if not (self.gen_create and self.gen_init):
            return []

        self.generate_adjacency_list()
        self.mt = [-1] * self.size

        for v in range(self.size):
            self.used = [False] * self.size
            self.khun(v)

        # Формируем список пар паросочетания
        matching = []
        for i in range(self.size):
            if self.mt[i] != -1:
                matching.append((self.mt[i], i))

        return matching

    def save_results(self, matching):
        """Сохранение в файл"""
        try:
            with open("kuhn_results.txt", "w", encoding="utf-8") as f:
                f.write("Матрица смежности графа:\n")
                for i in range(self.size):
                    for j in range(self.size):
                        f.write(f"{self.n[i][j]:3}")
                    f.write("\n")

                f.write("\nНаибольшее паросочетание:\n")
                for pair in matching:
                    f.write(f"{pair[0]} - {pair[1]}\n")

                f.write(f"\nВсего найдено {len(matching)} пар\n")
            print("Результаты сохранены в файл 'kuhn_results.txt'")
        except Exception as e:
            print(f"Ошибка при сохранении файла: {e}")


def main():
    kuhn = KuhnAlgorithm()

    while True:
        print("\n" + "=" * 50)
        print("АЛГОРИТМ КУНА ДЛЯ ПОИСКА НАИБОЛЬШЕГО ПАРОСОЧЕТАНИЯ")
        print("=" * 50)
        print("1. Задать размер графа (N×N)")
        print("2. Заполнить граф случайными значениями")
        print("3. Заполнить граф вручную")
        print("4. Вывести матрицу графа")
        print("5. Найти наибольшее паросочетание (алгоритм Куна)")
        print("0. Выход")
        print("-" * 50)

        try:
            choice = input("Выберите действие: ").strip()

            if choice == "1":
                try:
                    size = int(input("Введите количество вершин (больше 2): "))
                    if size > 2:
                        kuhn.create_matrix(size)
                        print(f"Граф размером {size}×{size} создан!")
                    else:
                        print("Размер должен быть больше 2!")
                except ValueError:
                    print("Ошибка: введите целое число!")

            elif choice == "2":
                if kuhn.gen_create:
                    if kuhn.initialize_random():
                        print("Граф заполнен случайными значениями!")
                        kuhn.print_matrix()
                else:
                    print("Сначала задайте размер графа!")

            elif choice == "3":
                if kuhn.gen_create:
                    if kuhn.initialize_manual():
                        print("Граф успешно введен!")
                        kuhn.print_matrix()
                else:
                    print("Сначала задайте размер графа!")

            elif choice == "4":
                if not kuhn.print_matrix():
                    print("Сначала создайте и заполните граф!")

            elif choice == "5":
                matching = kuhn.find_max_matching()
                if matching:
                    print("\n" + "=" * 50)
                    print("НАИБОЛЬШЕЕ ПАРОСОЧЕТАНИЕ:")
                    print("=" * 50)

                    kuhn.print_matrix()

                    print(f"\nНайдено пар: {len(matching)}")
                    print("Паросочетания (левая часть - правая часть):")
                    for pair in matching:
                        print(f"  {pair[0]} → {pair[1]}")

                    # Сохранение результатов
                    save = input("\nСохранить результаты в файл? (да/нет): ").strip().lower()
                    if save in ['да', 'yes', 'y', 'д']:
                        kuhn.save_results(matching)
                else:
                    print("Сначала создайте и заполните граф!")

            elif choice == "0":
                print("Выход из программы...")
                break

            else:
                print("Неверный выбор! Попробуйте снова.")

        except KeyboardInterrupt:
            print("\nПрограмма прервана пользователем.")
            break
        except Exception as e:
            print(f"Произошла ошибка: {e}")


if __name__ == "__main__":
    main()
