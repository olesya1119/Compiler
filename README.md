# Вариант задания

Создание функции языка Go
func calc(a, b, c int) {
    return a + b * (c - b)
};

# Примеры допустимых строк

1)	“func calc(a, b, c int) { return a + b * (c - 10) };”
![image](https://github.com/user-attachments/assets/c88bac5d-5652-4d6c-a0b9-c899d4e48c8e)

2)	“func super(fn, wet int) { return 15 - qq / fn * 35 };”   
![image](https://github.com/user-attachments/assets/1c2ff654-8f10-4699-912e-c778cc822976)

3)	“func super(fn wet int) { retrn 15 - qq / fn * 35};”  
![image](https://github.com/user-attachments/assets/a4e050a6-2a53-48c1-a411-db8b00017a3c)

4)	“fnc supe^r(fn, wet, it) {return fn * (wet - it};” 
![image](https://github.com/user-attachments/assets/66a53898-3150-4381-a6bc-ded783eb05cc)

5)	“func &&& super(fn, gfg wet int) {retrn fn * (wet - it)}” 
![image](https://github.com/user-attachments/assets/902d530f-cd19-4d5e-bb5c-9b5981e8f5f5)

# Разработанная грамматика

Определим грамматику создания функции языка Go G[<F>] в нотации Хомского с продукциями P:
```
G[<F>]:
1) <F> -> 'func ' <I>
2) <I> -> id <Q>
3) <Q> -> '(' <M>
4) <M> -> <P> <L>
5) <P> -> id ' ' type | id ',' <P> 
6) <L> -> ')' <K>
7) <K> -> '{' <R>
8) <R> -> 'return '<RV>
9) <RV> -> <E><EB>
10) <EB> -> '}'<END>
11) <E> -> <T> <A>
12) <A> -> '+' <T> <A> | '-' <T> <A> | ε
13) <T> -> <O> <B>
14) <B> -> '*' <O> <B> | '/' <O> <B> | ε
15) <O> -> id | num | '('<E>')'
16) <END> -> ';'
17) <IDENTIFIER> -> letter <IDENTIFIER_REM>
18) <IDENTIFIER_REM> -> letter | digit | ε
19) <NUMBER> -> digit <NUMBER_TAIL>
20) <NUMBER_TAIL> -> digit <NUMBER_TAIL> | ε
‒ num = <NUMBER>
‒ id = <IDENTIFIER>
‒ letter -> 'a' | 'b' | ... | 'z' | 'A' | 'B' | ... | 'Z'
‒ digit -> '0' | '1' | ... | '9'
‒ type -> ' int' | ' uint' | ' float32' | ' float64'
Следуя введенному формальному определению грамматики, представим G[<F>] ее составляющими:
‒ Z = <F>
‒ VT = {a, b, ..., z, A, B, ..., Z, 0, 1, ...,9, +, -, /, *, {, }, (, ), ;}
‒ VN = {<F>, <I>, <Q>, <M>, <P>, <L>, <K>, <R>, <RV>, <EB>, <EB>, <E>, <A>, <T>, <B>, <O>, <END>, <IDENTIFIER>, <IDENTIFIER_REM>, <NUMBER>, <NUMBER_TAIL>}.
```
# Классификация грамматики

Согласно классификации Хомского, полученная порождающая грамматика G[<F>] соответствует типу контекстно-свободных, так как правая часть каждой редукции начинается либо с терминального символа, либо с нетерминального, принадлежащего объединённому словарю.
```
A →a,A∈V_N,a∈V^*.
Грамматика G[<F>] не является автоматной, так как не все её редукции начинаются с терминального символа. По этой же причине данная грамматика не является S - грамматикой.```
![image](https://github.com/user-attachments/assets/9b90ec42-779f-4e45-b9e0-e935acf77b66)


# Справочная система

Здесь вы найдете информацию о всех доступных функциях и возможностях приложения.

## Функционал программы

В программе реализован следующий функционал:

### Файл
- **Создать** - Открывает новый пустой документ.
- **Открыть** - Открывает диалоговое окно для выбора файла.
- **Сохранить** - Сохраняет текущий документ.
- **Сохранить как** - Открывает диалоговое окно сохранения файла.
- **Выход** - Закрытие программы.

### Правка
- **Отменить** - Отменяет последнее действие.
- **Повторить** - Повторяет отменённое действие.
- **Копировать** - Копирует выделенный текст.
- **Вырезать** - Вырезает выделенный текст.
- **Вставить** - Вставляет текст из буфера обмена.
- **Удалить** - Удаляет выделенный фрагмент или символ.
- **Выделить все** - Выделяет весь текст в документе.

### Справка
- **Справка** - Открывает справочную систему.
- **О программе** - Информация о программе.

## Как пользоваться программой

1. **Создание нового документа**: Нажмите на иконку "Создать" или выберите соответствующий пункт в меню "Файл".
2. **Открытие существующего документа**: Используйте иконку "Открыть" или меню "Файл" для выбора файла.
3. **Сохранение документа**: Нажмите на иконку "Сохранить" или выберите "Сохранить как" для сохранения файла в новом месте.
4. **Редактирование текста**: Используйте функции из меню "Правка" для копирования, вставки, вырезания и других операций с текстом.
5. **Получение справки**: Если у вас возникли вопросы, нажмите на иконку "Справка" или выберите соответствующий пункт в меню.

## Иконки и их значение

- 📄 **Создать**: Создание нового документа.
- 📂 **Открыть**: Открытие существующего документа.
- 💾 **Сохранить**: Сохранение текущего документа.
- ↩️ **Отменить**: Отмена последнего действия.
- ↪️ **Повторить**: Повтор отменённого действия.
- 📋 **Копировать**: Копирование выделенного текста.
- ✂️ **Вырезать**: Вырезание выделенного текста.
- 📎 **Вставить**: Вставка текста из буфера обмена.
- ❓ **Справка**: Открытие справочной системы.
- ℹ️ **О программе**: Информация о программе.

# Сканер
## Постановка задачи:
Изучить назначение лексического анализатора. Спроектировать алгоритм и выполнить программную реализацию сканера.

## Персональный вариант задания на лабораторную работу
Создание функции языка Go

## Примеры допустимых строк
func calc(x, y, z int) int {
    return x + (y * z)
};
![image](https://github.com/user-attachments/assets/87f09420-2a9f-4107-bd10-1f7ceb092b87)

## Диаграмма состояний сканера 
![Диаграмма](https://github.com/user-attachments/assets/e1141303-5334-4763-be74-c511e089a62c)


