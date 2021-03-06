# Оптимизация присваивания собственного значения !heading

### Команда BOOM

#### Постановка задачи
Требуется заменить в AST-дереве присваивания вида a = a на null.

#### Зависимости задач в графе задач
Задача зависит от:
* генерация AST-дерева;
* заполнение связей типа "предок-потомок".

#### Теория
В исходном коде программы иногда можно встретить присваивания переменной самой себе. По сути после выполнения такого кода ничего не меняется, значит от такого кода можно и нужно избавляться. На этапе компиляции можно выявить такие дефекты и произвести оптимизацию.

#### Особенности реализации
При использовании программного кода, позволяющего произвести оптимизацию устранения выражений вида a = a, требуется произвести построение AST-дерева для исходного кода программы, написанном на реализованном языке программирования, и выделить для построенного дерева связи типа "предок-потомок". К полученному дереву следует применить визитор SimpleLang.Visitors.AssignVisitor. Таким образом, последовательность действий для использования данной оптимизации будет следующей:
1) сгенерировать AST-дерево;
2) с помощью визитора SimpleLang.Visitors.FillParentVisitor заполнить связи между узлами дерева типа "предок-потомок";
3) создать объект типа SimpleLang.Visitors.AssignVisitor;
4) для полученного дерева вызвать функцию Visit с объектом AssignVisitor, созданном на 3-ем этапе, поданным как аргумент функции.

Для решения этой задачи был реализован класс AssignVisitor, который переопределяет процедуру VisitAssignNode.
```csharp
class AssignVisitor: ChangeVisitor
    {
        public override void VisitAssignNode(AssignNode a)
        {
            if ((a.Expr is IdNode) && String.Equals(a.Id.Name, (a.Expr as IdNode).Name))
            {
                ReplaceStat(a, new NullNode());
            }
            else
            {
                base.VisitAssignNode(a);
            }
        }

        public override void VisitIfNode(IfNode ifn)
        {
            ifn.Cond.Visit(this);
        }
        public override void VisitWhileNode(WhileNode w)
        {
            w.Expr.Visit(this);
        }

        public override string ToString()
        {
            return "";
        }
    }
```


#### Тесты
Из исходной программы вида
```csharp
{
    int a, b;
    int c, d;
    a = 3;
    b = a + 3;
    c = b + a + 2;
    d = c + b;
    c = c;
    b = a + 2;
    d = d;
    a = b + 1;
}

```

Мы получаем
```csharp
{
    int a, b;
    int c, d;
    a = 3;
    b = a + 3;
    c = b + a + 2;
    d = c + b;
    #NULL;
    b = a + 2;
    #NULL;
    a = b + 1;
}

```

[Вверх](#содержание)