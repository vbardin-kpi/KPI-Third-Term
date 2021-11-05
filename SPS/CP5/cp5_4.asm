IDEAL
MODEL SMALL
STACK 512

MACRO init
	mov ax, @data
	mov ds, ax
	mov es, ax 
ENDM init

MACRO start_macro
    mov dx, offset start111 
	call output_str
	mov cx, 16
	xor si, si
	l1:
		call input
		mov dx, offset new_line 
	    call output_str
		mov [ds:10h+si], ax	
		add si, 2
		loop l1		
	call show_result
ENDM start_macro

MACRO new_line_macro
	mov dx, offset separator 
	call output_str
ENDM new_line_macro

MACRO part1
	mov cx, 16
	xor si, si
	xor ax,ax
	l3:
		add ax, [ds:10h+si]
		add si, 2
		loop l3
	mov dx, offset Task__1 
	call output_str
	call output_register
    mov dx, offset new_line 
	call output_str
ENDM part1

MACRO part4
	mov dx, offset Task__4 
	call output_str
	
	MOV dx, offset X
	MOV al,7                
	PUSH cx                
	MOV cx,ax              
	MOV ah,0Ah              ;ввод строки в буфер
	MOV [X],al         
	MOV [X+1],0    
	MOV dx,offset X           
	INT 21h 
	MOV al,[X+1]      
	add dx,2                
	MOV ah,ch            
	POP cx  	
	CALL str_transformation  
	
	mov cx, 16
	xor si, si
	l5:
		mov bx,[ds:10h+si] 	
		push ax
		cmp ax,bx
		je point1
		jmp point2
	point1:
		mov ax, si
		add ax, 2
		mov bl, 2
		div bl
		mov bl, 4
		div bl
		xor bx,bx
		mov bl, ah 
		xor ah, ah 
		add al, 1
		
		cmp bl, 0
		je point3
		jmp point5
	point3:
		sub al,1
		mov bl, 4
		jmp point5
		
	point5:
		mov dx, offset new_line 
		call output_str
		call output_register
	point5_5:	
		xor ax,ax
		mov al, bl 
		call output_register		
		mov dx, offset new_line 
		call output_str
	point2:
	pop ax
		add si, 2
		loop l5	
ENDM part4

MACRO part3
	lea si, matrix_main
	mov cx, Len
	mov dx, offset Task__3 
	call output_str
	push ax
    push bx
    push cx
    push dx
    push si
    push di
	call sort 
    pop di
    pop si
    pop dx
    pop cx
    pop bx
    pop ax
	call show_result
ENDM part3

MACRO part2
    mov dx, offset Task__2_min 
	call output_str
	mov ax, [ds:10h]
	call output_register
    mov dx, offset new_line 
	call output_str

    mov dx, offset Task__2_max 
	call output_str
	mov ax, [ds:2Eh]
	call output_register
    mov dx, offset new_line
	call output_str
ENDM part2

MACRO exit_macro
exit:	
	mov ah,04Ch
	mov al,0 
	int 21h ; виклик функції DOS 4ch
ENDM exit_macro

MACRO show_result_macro
	mov cx, 4
	xor si, si
	mov di, offset buffe01
	l6:
		mov ax,[ds:10h+si]
		call output_register_ax
		add si, 2
		add di, 4
		loop l6
	mov [ds:090h], 0A0Dh
	mov [ds:092h], 24h
	mov [ds:di-1h],' '
	
    mov dx, offset buffe01
  	mov ah,09h
	int 21h
	
	mov cx, 4	
	mov di, offset buffe01
	push si
	push cx
	push dx
	mov cx, 20h
	xor si, si
	loop1111:
		mov [ds:076h+si], ' '
		add si, 2
		loop loop1111
	pop dx
	pop cx
	pop si
	loop2222:
		mov ax,[ds:10h+si]
		call output_register_ax
		add si, 2
		add di, 4
		loop loop2222
	mov [ds:090h], 0A0Dh
	mov [ds:092h], 24h
	mov [ds:di-1h],' '
    mov dx, offset buffe01
  	mov ah,09h
	int 21h
	mov cx, 4	
	mov di, offset buffe01
	loop3333:
		mov ax,[ds:10h+si]
		call output_register_ax
		add si, 2
		add di, 4
		loop loop3333
	mov [ds:090h], 0A0Dh
	mov [ds:092h], 24h
	mov [ds:di-1h],' '
    mov dx, offset buffe01
  	mov ah,09h
	int 21h
	mov cx, 4	
	mov di, offset buffe01
	loop4444:
		mov ax,[ds:10h+si]
		call output_register_ax
		add si, 2
		add di, 4
		loop loop4444
	mov [ds:090h], 0A0Dh
	mov [ds:092h], 24h
	mov [ds:di-1h],' '
    mov dx, offset buffe01
  	mov ah,09h
	int 21h
	mov cx, 4	
	mov di, offset buffe01
	ret
ENDM show_result_macro

MACRO output_register_macro
    mov [ES:0235h],' '
	mov [ES:0234h],' '
	mov [ES:0233h],' '
	mov [ES:0232h],' '
	mov [ES:0231h],' '
    mov di,0230h    
    push cx 
    push dx
    push bx
    mov bx,10   
    XOR CX,CX   
point_01:   XOR dx,dx
    DIV bx      
    PUSH DX     
    INC CX
    TEST AX,AX
    JNZ point_01
point_012:   POP AX
    ADD AL,'0'  
    STOSb       
    LOOP point_012       
    pop bx      
    POP dx
    POP cx 
mov [ES:0235h],'$'
    mov dx, 230h 
  	mov ah,09h
	int 21h
ret 
ENDM output_register_macro

MACRO str_transformation_macro
    PUSH bx                
    PUSH dx

    test al,al              ;Проверка длины строки
    jz point_str2         
    MOV bx,dx              
    MOV bl,[bx]             
    CMP bl,'-'              ;проверка знака
    jne point_str1       
    inc dx                  
    dec al                  
point_str1:
    CALL save_input   
    jc point_str2         	
    CMP bl,'-'             
    jne point_str3          ;Если первый символ не '-', то число положительное
    CMP ax,32734            ;проверка модуля числа
    ja point_str2          
    JMP point_str4            
point_str3:
    CMP ax,32767            ;проверка модуля числа
    ja point_str2          
point_str4:
    clc                     
    JMP point_str5          
point_str2:
    MOV dx,offset uncorrect ;; Закоментовані повідомлення у ході налаштування
	MOV ah,9
	INT 21h
	XOR dx, dx
    XOR ax,ax               ;AX = 0
	CALL exit
point_str5:
    POP dx                  ;Восстановление регистров
    POP bx
    RET 
ENDM str_transformation_macro

MACRO save_input_macro
    PUSH cx                 ;Сохранение всех используемых регистров
    PUSH dx
    PUSH bx
    PUSH si
    PUSH di
 
    MOV si,dx             
    MOV di,10               
    MOV cl,al            
    JCXZ end9        ;Если длина = 0, возвращаем ошибку
    XOR ax,ax               
    XOR bx,bx             
 
loop454:
    MOV bl,[si]             
    INC si                 
    CMP bl,'0'             
    JL end9         
    CMP bl,'9'            
    JG end9         
    SUB bl,'0'              ;Преобразование символа-цифры в число
    MUL di                
    JC end9         
    ADD ax,bx        
    JC end9          ;Если переполнение - ошибка
    LOOP loop454          
    JMP exit21          
 
end9:
    XOR ax,ax            
    STC                     
 
exit21:
    POP di                  ;Восстановление регистров
    POP si
    POP bx
    POP dx
    POP cx
    ret 
ENDM save_input_macro

MACRO sort_macro
    mov bx, si
    mov dx, cx
    dec dx
    shl dx, 1               
    dec cx                      
    mov si, 0
point_start: mov di, dx             
point121: mov ax, [bx+di-2]       
    cmp ax, [bx+di]
    jbe f_2 
    xchg ax, [bx+di]         
    xchg ax, [bx+di-2]       
    xchg ax, [bx+di]         
f_2: sub di, 2              
    cmp di, si             
    ja point121
    add si, 2              
    loop point_start
    ret
ENDM sort_macro

MACRO output_str_macro
 push ax
 mov ah,9
 int 21h
 xor dx, dx
 pop ax
 ret
ENDM output_str_macro

MACRO input_macro
    PUSH dx                
    MOV al,7                
    PUSH cx                
    MOV cx,ax              
    MOV ah,0Ah             
    MOV [X],al         
    MOV [X+1],0    
    MOV dx,offset X          
    INT 21h 
    MOV al,[X+1]       
    add dx,2                
    MOV ah,ch               
    POP cx  	
	CALL str_transformation
    POP dx                  
    RET 
ENDM input_macro

MACRO output_register_ax_macro
    mov [ds:di+0Bh],20h
	mov [ds:di+0Ah],20h
	mov [ds:di+9h],20h
	mov [ds:di+8h],20h
	mov [ds:di+7h],20h
	mov [ds:di+6h],20h
	mov [ds:di+5h],20h
	mov [ds:di+4h],20h
	mov [ds:di+3h],20h
	mov [ds:di+2h],20h
	mov [ds:di+1h],20h
    push cx ;сохраняем регистры
    push dx
    push bx
    mov bx,10   
    XOR CX,CX   
circle1:   XOR dx,dx
    DIV bx      ;делим число на степени 10
    PUSH DX     
    INC CX
    TEST AX,AX
    JNZ circle1
circle2:   POP AX
    ADD AL,'0'  ;преобразовываем число в ASCII символ
    STOSb       
    LOOP circle2       
    pop bx      ;восстанавливаем регистры
    POP dx
    POP cx 
ret 
ENDM output_register_ax_macro

DATASEG
X DB "             ", 13, 10, '$'
matrix_main dw 0h, 0h ,0h ,0h
      dw 0h, 0h ,0h ,0h
      dw 0h, 0h ,0h ,0h
      dw 0h, 0h ,0h ,0h
uncorrect DB "uncorrect data!", 13, 10, '$' 
start111 DB "input numbers:     ", 13, 10, '$' 
buffe01 DB "                                                                                         ", 13, 10, '$' 
temp DB "///////", 13, 10, '$' 
Task__4 DB "Task 4 - input number:     ", 13, 10, '$' 
Task__3 DB "Task 3 - final Task__3 matrix:", 13, 10, '$' 
Task__2_min DB "Task 2 - min number: ",'$' 
Task__2_max DB "Task 2 - max number: ",'$'
Task__1 DB "Task 1 - sum: ", '$' 
new_line DB "", 13, 10, '$'  
separator DB "//*********||*********\\",13, 10,  '$' 

Len dw 16 




CODESEG
Start:

proc main
	init
	
	start_macro
	
	new_line_macro
	part1
	
	new_line_macro
	part4
	
	new_line_macro
	part3
	
	new_line_macro
	part2
	
	exit_macro
endp main 



PROC show_result 
	show_result_macro
ENDP show_result 


PROC output_register  
	output_register_macro
ENDP output_register  


PROC str_transformation
	str_transformation_macro
ENDP


PROC save_input
	save_input_macro
ENDP

proc sort  
	sort_macro
endp sort

PROC output_str  
	output_str_macro
ENDP output_str  

PROC input
	input_macro
ENDP
PROC output_register_ax  
	output_register_ax_macro
ENDP output_register_ax  



end Start