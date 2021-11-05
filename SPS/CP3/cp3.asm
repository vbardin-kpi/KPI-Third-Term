IDEAL
MODEL SMALL
STACK 512


MACRO main_read
; Початок макросу
	MOV bx, [ds:si] ;занесення в ax чисельного значення
	MOV bl, bh
	CMP bl, 030h ; c ascii = 2SDh ; Вибір відповідної функції
	jl exit22;
	CMP bl, 039h ; c ascii = 2SDh ; Вибір відповідної функції
	ja exit22;
	MOV bh, '0'
ENDM main_read



DATASEG;III.ПОЧАТОК СЕГМЕНТУ ДАНИХ


X DB "             ", 13, 10, '$' 
Y DB "             ", 13, 10, '$' 

out_of_bounds DB "incorrect number!", 13, 10, '$' 
final_message DB "result:       ", 13, 10, '$' 

input1 DB "please, input X", 13, 10, '$' 
input2 DB "please, input Y", 13, 10, '$'
result_out_of_bounds DB "result out of bounds!", 13, 10, '$'
final_message_rest DB "rest:             ", 13, 10, '$' 

CODESEG
Start:

	MOV ax, @data 
	MOV ds, ax 
	MOV es, ax 
	
	MOV dx, offset input1 ;; Закоментовані повідомлення у ході налаштування
	MOV ah,9
	INT 21h
	XOR dx, dx

	
	MOV dx, offset X
    PUSH dx                
    MOV al,7                
    PUSH cx                
    MOV cx,ax              
    MOV ah,0Ah              ;ввод строки в буфер
    MOV [X],al         
    MOV [X+1],0    
    MOV dx,offset X           ;DX = aдрес буфера
    INT 21h 
    MOV al,[X+1]       ;AL = длина введённой строки
    add dx,2                ;DX = адрес строки
    MOV ah,ch               ;Восстановление AH
    POP cx  	
	CALL str_transformation   ;Преобразование строки в слово (со знаком)     
	MOV dx, offset input2 ;; Закоментовані повідомлення у ході налаштування
	
	MOV ah,9
	INT 21h
	XOR dx, dx
	MOV dx, offset Y
    MOV al,7                
    PUSH cx                
    MOV cx,ax              
    MOV ah,0Ah              ;ввод строки в буфер
    MOV [Y],al         
    MOV [Y+1],0    
    MOV dx,offset Y           ;DX = aдрес буфера
    INT 21h 
    MOV al,[Y+1]       ;AL = длина введённой строки
    add dx,2                ;DX = адрес строки
    MOV ah,ch               ;Восстановление AH
    POP cx  	
	CALL str_transformation   ;Преобразование строки в слово (со знаком)
    POP dx 
	
	
	MOV si, 1Bh
	call read_number
	CMP ax, 0
	JE general
	
	MOV si, 0Bh
	call read_number
	
    MOV bx,[ds:02h]               ;BL = первый символ строки
    CMP bl,'-'              ;Сравнение первого символа с '-'	
	JE negative
	JMP positive
negative:
	CMP ax, 5
	JE f_1
	JMP general
f_1:
    MOV bx,[ds:12h]               ;BL = первый символ строки
    CMP bl,'-' 
	JE case1
	JMP general	
positive:
	CMP ax, 3
	JG f_2
	JMP general
f_2:
    MOV bx,[ds:12h]               ;BL = первый символ строки
    CMP bl,'-' 
	JE general
	JMP case2	
general:
	MOV ax, 1
	CALL print
	JMP exit
case1:
	MOV si, 1Bh
	CALL read_number
	MOV bx, ax
	MOV ax, 200
	DIV bx
	
	MOV bx, offset final_message
	MOV [bx+8], '-'
	CALL print
	CALL print_rest
	CALL exit
case2:
	MOV bx, 6
	MUL bx
	
	CMP dx, 0
	JG error_11 
	JMP skip_error_11
error_11:
    MOV dx, offset result_out_of_bounds
	MOV ah,9
	INT 21h
	XOR dx, dx
	CALL exit
skip_error_11:
	CALL print
	CALL exit

exit:	
	MOV ah,04Ch
	MOV al,0 ; отримання коду виходу
	INT 21h ; виклик функції DOS 4ch



PROC save_input
    PUSH cx                 ;Сохранение всех используемых регистров
    PUSH dx
    PUSH bx
    PUSH si
    PUSH di
 
    MOV si,dx               ;SI = адрес строки
    MOV di,10               ;DI = множитель 10 (основание системы счисления)
    MOV cl,al             ;CX = счётчик цикла = длина строки     movzx cx,al             ;CX = счётчик цикла = длина строки
    JCXZ error_end        ;Если длина = 0, возвращаем ошибку
    XOR ax,ax               ;AX = 0
    XOR bx,bx               ;BX = 0
 
loop21:
    MOV bl,[si]             ;Загрузка в BL очередного символа строки
    INC si                  ;Инкремент адреса
    CMP bl,'0'              ;Если код символа меньше кода '0'
    JL error_end          ; возвращаем ошибку
    CMP bl,'9'              ;Если код символа больше кода '9'
    JG error_end          ; возвращаем ошибку
    SUB bl,'0'              ;Преобразование символа-цифры в число
    MUL di                  ;AX = AX * 10
    JC error_end          ;Если результат больше 16 бит - ошибка
    ADD ax,bx               ;Прибавляем цифру
    JC error_end          ;Если переполнение - ошибка
    LOOP loop21           ;Команда цикла
    JMP exit21          ;Успешное завершение (здесь всегда CF = 0)
 
error_end:
    XOR ax,ax               ;AX = 0
    STC                     ;CF = 1 (Возвращаем ошибку)
 
exit21:
    POP di                  ;Восстановление регистров
    POP si
    POP bx
    POP dx
    POP cx
    ret 
ENDP

PROC read_error
    MOV dx, offset out_of_bounds ;; Закоментовані повідомлення у ході налаштування
	MOV ah,9
	INT 21h
	XOR dx, dx
    XOR ax,ax               ;AX = 0
	CALL exit
	RET 
ENDP
PROC read_number
	PUSH cx
	PUSH dx
	PUSH bx
    XOR ax, ax
loop1:
	MOV bx, [ds:si] ;занесення в ax чисельного значення
	MOV bl, bh
    XOR bh,bh
	CMP bl, 030h ; c ascii = 2SDh ; Вибір відповідної функції
	jl skip1;
	CMP bl, 039h ; c ascii = 2SDh ; Вибір відповідної функції

	main_read
	sub bl, bh 
	MOV al,bl
	dec si
	
	main_read
	sub bl, bh
	MOV cx, ax
	MOV ax, 10
	XOR bh,bh
	MUL bx
	add ax, cx
	dec si

	main_read
	sub bl, bh
	MOV cx, ax
	MOV ax, 100
	XOR bh,bh
	MUL bx
	add ax, cx
	dec si
	
	main_read
	sub bl, bh
	MOV cx, ax
	MOV ax, 1000
	XOR bh,bh
	MUL bx
	add ax, cx
	dec si
	JMP last_check
	
skip1:
	dec si
	JMP loop1
	

exit22:
XOR dx, dx
POP bx
POP dx
POP cx
	RET 
last_check:
	main_read
	sub bl, bh
	MOV cx, ax
	MOV ax, 10000
	XOR bh,bh
	MUL bx
	add ax, cx
	dec si
	JMP exit22
ENDP


PROC str_transformation
    PUSH bx                
    PUSH dx

    test al,al              ;Проверка длины строки
    jz f2         
    MOV bx,dx              
    MOV bl,[bx]             
    CMP bl,'-'              ;проверка знака
    jne f1       
    inc dx                  
    dec al                  
f1:
    CALL save_input   
    jc f2         	
    CMP bl,'-'             
    jne f3          ;Если первый символ не '-', то число положительное
    CMP ax,32734            ;проверка модуля числа
    ja f2          
    JMP f4            
f3:
    CMP ax,32767            ;проверка модуля числа
    ja f2          
f4:
    clc                     
    JMP f5          
f2:
	CALL read_error
f5:
    POP dx                  ;Восстановление регистров
    POP bx
    RET 
ENDP

PROC print
	PUSH dx 
    MOV di,offset final_message   
	add di,9
    PUSH cx 
    PUSH bx
    MOV bx,10   
    XOR CX,CX 
print1:   XOR dx,dx
    DIV bx     
    PUSH DX     
    INC CX
    TEST AX,AX
    JNZ print1
print2:   POP AX
    ADD AL,'0'  
    STOSb      
    LOOP print2       
    POP bx      
    POP cx 
	jc print3
	JMP print4
print3:
	MOV dx, offset result_out_of_bounds 
	MOV ah,9
	INT 21h
	XOR dx, dx
	CALL exit
print4:
    MOV dx, offset final_message 
	MOV ah,9
	INT 21h
	XOR dx, dx
	POP dx
RET  
ENDP print


PROC print_rest
	MOV ax, dx
    MOV di,offset final_message_rest   
	add di, 7
    PUSH cx 
    PUSH dx
    PUSH bx
    MOV bx,10  
    XOR CX,CX   
rest1:   XOR dx,dx
    DIV bx      
    PUSH DX     
    INC CX
    TEST AX,AX
    JNZ rest1
rest2:   POP AX
    ADD AL,'0'  
    STOSb       
    LOOP rest2      
	POP bx      
    POP dx
    POP cx 
    MOV dx, offset final_message_rest 
	MOV ah,9
	INT 21h
	XOR dx, dx
	RET 
ENDP print_rest 

end Start
