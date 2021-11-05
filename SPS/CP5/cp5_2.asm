IDEAL
MODEL SMALL
STACK 512


MACRO init
	mov ax,@data; @data ідентифікатор, що створюються директивою model
	mov ds, ax	; Завантаження початку сегменту даних в регістр ds
	mov es, ax	; Завантаження початку сегменту даних в регістр es
ENDM init

MACRO read_console_macro
	call input_sdec_word
	
    mov bx, [ds:0002] 
	cmp bl, 02Dh ; c ascii = 2Dh ; Вибір відповідної функції
	je negative;
	jmp positive;
ENDM read_console_macro

MACRO case1_macro
negative:
    cmp ax,67            ;Модуль отрицательного числа должен быть не больше 34
    ja negative_large
	jmp negative_small
ENDM case1_macro

MACRO case2_macro
positive:
    math_1_macro
    ;mov dx, offset display_message_1 ; Закоментовані повідомлення у ході налаштування
    ;call display
    jmp exit
ENDM case2_macro

MACRO negative_case_macro
negative_large:
    math_2_macro
    ;mov dx, offset display_message_1 ; Закоментовані повідомлення у ході налаштування
    ;call display
    jmp exit
negative_small:
    math_3_macro
    ;mov dx, offset display_message_1 ; Закоментовані повідомлення у ході налаштування
    ;call display
    jmp exit
ENDM negative_case_macro

MACRO exit_macro
exit:	
	mov ah,04Ch
	mov al,0 
	int 21h 
ENDM exit_macro

MACRO display_macro
	 mov ah,9
	 int 21h
	 xor dx, dx
	 ret
ENDM display_macro

MACRO input_str_macro
    push cx                 ;Сохранение СX
    mov cx,ax               ;Сохранение AX в CX
    mov ah,0Ah              ;Функция DOS 0Ah - ввод строки в буфер
    mov [buffer],al         ;Запись максимальной длины в первый байт буфера
    mov [buffer+1],0    ;Обнуление второго байта (фактической длины)
    mov dx,offset buffer           ;DX = aдрес буфера
    int 21h                 ;Обращение к функции DOS
    mov al,[buffer+1]       ;AL = длина введённой строки
    add dx,2                ;DX = адрес строки
    mov ah,ch               ;Восстановление AH
    pop cx                  ;Восстановление CX
    ret
ENDM input_str_macro

MACRO input_sdec_word_macro
    push dx                 ;Сохранение DX
    mov al,7                ;Ввод максимум 7 символов (-32768) + конец строки
    call input_str          ;Вызов процедуры ввода строки
	call str_to_sdec_word   ;Преобразование строки в слово (со знаком)
    pop dx                  ;Восстановление DX
    ret
ENDM input_sdec_word_macro

MACRO str_to_sdec_word_macro
    push bx                 ;Сохранение регистров
    push dx

    test al,al              ;Проверка длины строки
    jz stsdw_error          ;Если равно 0, возвращаем ошибку
    mov bx,dx               ;BX = адрес строки
    mov bl,[bx]             ;BL = первый символ строки
    cmp bl,'-'              ;Сравнение первого символа с '-'
    jne stsdw_no_sign       ;Если не равно, то преобразуем как число без знака
    inc dx                  ;Инкремент адреса строки
    dec al                  ;Декремент длины строки
stsdw_no_sign:
    call str_to_udec_word   ;Преобразуем строку в слово без знака
    jc stsdw_error         	;Если ошибка, то возвращаем ошибку
    cmp bl,'-'              ;Снова проверяем знак
    jne stsdw_plus          ;Если первый символ не '-', то число положительное
    cmp ax,32734            ;Модуль отрицательного числа должен быть не больше 32768 - 34
    ja stsdw_error          ;Если больше (без знака), возвращаем ошибку
;    neg ax                  ;Инвертируем число
    jmp stsdw_ok            ;Переход к нормальному завершению процедуры
stsdw_plus:
    cmp ax,32767            ;Положительное число должно быть не больше 32767
    ja stsdw_error          ;Если больше (без знака), возвращаем ошибку
 
stsdw_ok:
    clc                     ;CF = 0
    jmp stsdw_exit          ;Переход к выходу из процедуры
stsdw_error:
    mov dx, offset error_message ;; Закоментовані повідомлення у ході налаштування
    call display
    xor ax,ax               ;AX = 0
    ;stc                     ;CF = 1 (Возвращаем ошибку
	;jmp stsdw_exit          ;Переход к выходу из процедуры 
	jmp exit
stsdw_exit:
    pop dx                  ;Восстановление регистров
    pop bx
    ret
ENDM str_to_sdec_word_macro

MACRO str_to_udec_word_macro
    push cx                 ;Сохранение всех используемых регистров
    push dx
    push bx
    push si
    push di
 
    mov si,dx               ;SI = адрес строки
    mov di,10               ;DI = множитель 10 (основание системы счисления)
    mov cl,al             ;CX = счётчик цикла = длина строки     movzx cx,al             ;CX = счётчик цикла = длина строки
    jcxz studw_error        ;Если длина = 0, возвращаем ошибку
    xor ax,ax               ;AX = 0
    xor bx,bx               ;BX = 0
 
studw_lp:
    mov bl,[si]             ;Загрузка в BL очередного символа строки
    inc si                  ;Инкремент адреса
    cmp bl,'0'              ;Если код символа меньше кода '0'
    jl studw_error          ; возвращаем ошибку
    cmp bl,'9'              ;Если код символа больше кода '9'
    jg studw_error          ; возвращаем ошибку
    sub bl,'0'              ;Преобразование символа-цифры в число
    mul di                  ;AX = AX * 10
    jc studw_error          ;Если результат больше 16 бит - ошибка
    add ax,bx               ;Прибавляем цифру
    jc studw_error          ;Если переполнение - ошибка
    loop studw_lp           ;Команда цикла
    jmp studw_exit          ;Успешное завершение (здесь всегда CF = 0)
 
studw_error:
    xor ax,ax               ;AX = 0
    stc                     ;CF = 1 (Возвращаем ошибку)
 
studw_exit:
    pop di                  ;Восстановление регистров
    pop si
    pop bx
    pop dx
    pop cx
    ret
ENDM str_to_udec_word_macro

MACRO math_1_macro
	add ax,67     ;AX = AX + 34
	mov bx, offset result_buffer
	call output
    ret
ENDM math_1_macro

MACRO math_3_macro
    mov bx, ax
    mov ax, 67
    sub ax, bx
	mov bx, offset result_buffer
	mov [bx+8], ' '
	call output	
    ret
ENDM math_3_macro

MACRO math_2_macro
	sub ax,67     ;AX = AX + 34	
	mov bx, offset result_buffer
	mov [bx+8], '-'
	call output
    ret
ENDM math_2_macro

MACRO output_macro
    mov di,offset result_buffer    ;es:di - адрес буфера приемника
	mov cx, 9
	loop1:
	    INC di
	    loop loop1
    push cx ;сохраняем регистры
    push dx
    push bx
    mov bx,10   ;основание системы
    XOR CX,CX   ;в сх будет количество цифр в десятичном числе
m1:   XOR dx,dx
    DIV bx      ;делим число на степени 10
    PUSH DX     ;и сохраняем остаток от деления(коэффициенты при степенях) в стек
    INC CX
    TEST AX,AX
    JNZ m1
m2:   POP AX
    ADD AL,'0'  ;преобразовываем число в ASCII символ
    STOSb       ;сохраняем в буфер
    LOOP m2       ;все цифры
    pop bx      ;восстанавливаем регистры
    POP dx
    POP cx 

    mov dx, offset result_buffer ; Закоментовані повідомлення у ході налаштування
    call display
	call exit
ret 
ENDM output_macro

DATASEG;III.ПОЧАТОК СЕГМЕНТУ ДАНИХ

buffer db 254 
error_message DB "number out of bounds!", 13, 10, '$' 
result_buffer DB "result:       ", 13, 10, '$' 

CODESEG
Start:
	init
	read_console_macro

	case1_macro
	case2_macro
	negative_case_macro
	exit_macro

PROC display
	display_macro
ENDP display 


PROC input_str 
	input_str_macro
ENDP

PROC input_sdec_word
	input_sdec_word_macro
ENDP
	
PROC str_to_sdec_word
	str_to_sdec_word_macro
ENDP

PROC str_to_udec_word
	str_to_udec_word_macro
ENDP

PROC output
	output_macro
ENDP output
end Start