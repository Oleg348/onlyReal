# Определение всех естественных циклов в CFG с информацией об их вложенности !heading

### Команда Intel

#### Постановка задачи

Задача состояла определении всех естественных циклов в CFG и вывести информацию о вложенности.

##$# Зависимости задач в графе задач

Данная задача не зависит от других задач в графе задач и не порождает новых зависимостей.

##$# Теория

Для данного обратного ребра n → d определим естественный цикл ребра как d плюс множество узлов, которые могут достичь n не проходя через d. Если два и более циклов имеют один заголовок, то они объединяются в один цикл. Естественные циклы либо не пересекаются, либо один из них вложен в другой. 

##$# Особенности реализации

```
private static List<List<int>> GetListCycles(List<List<int>> list) {
            var way = new List<int>();
            var ways = new List<List<int>>();
            for (int i = 0; i < list.Count; i++) {
                for (int j = 0; j < list[i].Count; j++) {
                    if (list[i][j] < i) {
                        way = NaturalCycles.GetWay(list, list[i][j], i);
                        ways.Add(way);
                    }
                }
            }

            for (int i = 0; i < ways.Count; i++) {
                int j = 0;
                while (j < ways.Count) {
                    if ((ways[i][0] == ways[j][0]) && (i != j))
                        ways.RemoveAt(j);
                    else
                        j++;
                }
            }

            for (int i = 0; i < ways.Count; i++) {
                for (int j = 0; j < ways.Count; j++) {
                    var gg = ways[i].Intersect(ways[j]).ToList();
                    if ((gg.Count == 1) && (ways[i][ways[i].Count - 1] == ways[j][0])) {
                        for (int l = 1; l < ways[j].Count; l++) {
                            ways[i].Add(ways[j][l]);
                        }
                    } else {
                        if ((gg.Count != ways[j].Count) && (ways[i][0] < ways[j][0])) {
                            for (int l = 0; l < ways[j].Count; l++) {
                                if (!ways[i].Contains(ways[j][l]))
                                    ways[i].Add(ways[j][l]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < ways.Count; i++)
                ways[i].Sort();
            return ways;
        }
```
Конструктор SearchNaturalCycles класса NaturalCycles принимает в качестве аргумента ControlFlowGraph, обрабатывая List<List<int>> связей вершин графа, находит естественные циклы и возращает переменной, вызывавшей конструктор. Конструктор также выводит в консоль информацию об их вложенности. Для обработки связей вершин графа используется функция GetListCycles, которая находит обратные дуги и с помощью функции GetWay выводит вершины, через которые можно попасть от начала к концу цикла. После этого полученные циклы обрабатываются: если конец одного цикла = началу другого, значит первый цикл больше, чем получилось изначально. Если циклы пересекаются (но не вложены), то цикл также дополняется.  

##$# Тесты

Программа до применения оптимизации:
```
{
	bool w;
	w = (1 < 2) && (3 < 5);
	int a;
	int b;
	int c;
	a = 0;
	b = 1;
	while (w) {
		c = a;		
		a = b;
		b = c + a;
		println(a);		
		w = (a < 1000000);
	}
}

```
Программа после применения оптимизации:
```
0: 1 3
```

[Вверх](#содержание)
