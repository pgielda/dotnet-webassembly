void sayc(int s);

void putss(char *s) {
	int i = 0;
	while(s[i] != 0) {
		sayc((int)s[i]);
		i++;
	}
}

int main() {
	putss("hello!\n");
}

int  test() {
	putss(__func__);
}
