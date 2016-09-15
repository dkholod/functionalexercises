import org.scalacheck._
import Prop.forAll

def badSort (xs :List[Int]) = xs

def quickSort (lst: List[Int]) : List[Int] = lst match {
  case Nil | _::Nil => lst
  case x::xs => {
      val (l, g)= xs partition(x >= _)
      quickSort(l) ++ List(x) ++ quickSort(g)
  }
}

class SortSpec(sortFn: List[Int] => List[Int]) extends Properties("list is sorted") {
  val sort = sortFn

  property("sorted list should have same length as original") = forAll {
    (lst: List[Int]) => sort(lst).length == lst.length
  }

  property("neighbour pairs from a list should be ordered") = forAll {
    (lst: List[Int]) => sort(lst) match {
        case Nil | _::Nil => true
        case x::xs => (x::xs).zip(xs).forall(it => it._1 <= it._2)
        }
  }
}

def time[R] (exec: => R): R = {
    val start = System.nanoTime()
    val r = exec
    val stop = System.nanoTime()
    val elapsed = (stop - start) / 1000000000.0
    println(s"Elapsed time: $elapsed sec")
    r
  }

time {
  (new SortSpec(badSort)).check(Test.Parameters.default.withMinSuccessfulTests(100000))
  (new SortSpec(quickSort)).check(Test.Parameters.default.withMinSuccessfulTests(100000))
}