import java.util.concurrent.atomic.*;

public class LockFreeStack<E> {
	
	private static class Node<T> {
		T value;
		Node<T> next;
		Node(T val) { value = val; next = null; }
	}
	
	private final AtomicReference<Node<E>> headRef =
		new AtomicReference<>();
		
	public void push(E val) {
		if (val == null)
			throw new IllegalArgumentException("val cannot be null");
		Node<E> oldHead, newHead = new Node<>(val);
		do {
			oldHead = headRef.get();
			newHead.next = oldHead;
		} while (!headRef.compareAndSet(oldHead, newHead));
	}
	
	public E tryPop() {
		Node<E> oldHead, newHead;
		do {
			oldHead = headRef.get();
			if (oldHead == null)
				return null;
			newHead = oldHead.next;
		} while (!headRef.compareAndSet(oldHead, newHead));
		return oldHead.value;
	}
}
