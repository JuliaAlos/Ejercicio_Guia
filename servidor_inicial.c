#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>

int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(9050);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0)
		printf("Error en el Listen");
	
	// Atenderemos solo 5 peticione
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		//sock_conn es el socket que usaremos para este cliente
		
		//Bucle de atencion al cliente
		int terminar =0;
		while (terminar==0)
		{
			// Ahora recibimos su nombre, que dejamos en peticion
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibido\n");
			
			// Tenemos que a?adirle la marca de fin de string 
			// para que no escriba lo que hay despues en el peticion
			peticion[ret]='\0';
			
			//Escribimos el nombre en la consola
			
			printf ("Se ha conectado: %s\n",peticion);
			
			
			char *p = strtok( peticion, "/");
			int codigo =  atoi (p);
			p = strtok( NULL, "/");
			char nombre[20];
			if (codigo == 0)
			{
				terminar =1;
			}
			else
			{
				strcpy (nombre, p);
				printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
				
				if (codigo ==1) //piden la longitd del nombre
					sprintf (respuesta,"%d",strlen (nombre));
				else if (codigo ==2)
				{
					// quieren saber si el nombre es bonito
					if((nombre[0]=='M') || (nombre[0]=='S'))
						strcpy (respuesta,"SI");
					else if (strcmp(nombre,"Julia")==0)
						strcpy (respuesta,"J�lia tu eres mi reina");
					else
						strcpy (respuesta,"NO");
					
					
					printf ("%s\n", respuesta);
					// Y lo enviamos
				}
				else
				{
					p=strtok(NULL,"/");
					float altura =  atof (p);
					printf("Tu altura es %.2f\n",altura);
					if (altura > 1.70){
						printf("%s eres alto ",nombre);
						sprintf (respuesta,"%s eres alto ",nombre);
					}
					else {
						printf("% no eres alto ",nombre);
						sprintf (respuesta,"%s no eres alto ",nombre);
					}
				}
				write (sock_conn,respuesta, strlen(respuesta));
			}
		}
		
		// Se acabo el servicio para este cliente
		close(sock_conn); 
		
	}
}
