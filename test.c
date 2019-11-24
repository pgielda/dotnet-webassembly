extern void sayc(int s);
extern void greet();

void putss(char *s) {
	int i = 0;
	while(s[i] != 0) {
		sayc((int)s[i]);
		i++;
	}
}

void main() {
	putss("hello!\n");
	greet();
}
