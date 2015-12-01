public class IsConst {
	
	private final int VAL1 = getVal2();
	private final int VAL2 = getFive();
	private final int VAL3 = VAL2;

	private final int VAL4;
	private final int VAL5;
	private final int VAL6;

	private int getVal2() { return VAL2; }
	private int getFive() { return 5; }
	private int getVal5() { return VAL5; }
	
	public IsConst() {
		VAL4 = getVal5();
		VAL5 = 5;
		VAL6 = VAL5;
	}
	
	public void print123() {
		System.out.printf("VAL1: %d; ", VAL1);
		System.out.printf("VAL2: %d; ", VAL2);
		System.out.printf("VAL3: %d\n", VAL3);
	}
	
	public void print456() {
		System.out.printf("VAL4: %d; ", VAL4);
		System.out.printf("VAL5: %d; ", VAL5);
		System.out.printf("VAL6: %d\n", VAL6);
	}
	
	public static void main(String[] args) {
		IsConst ic = new IsConst();
		ic.print123();
		ic.print456();
	}
}