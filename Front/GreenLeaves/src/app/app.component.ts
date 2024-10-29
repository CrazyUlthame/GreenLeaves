import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { Component, ViewChild, ElementRef, HostListener  } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import {ConstantesURL}  from '../app/Service/Constantes';
import {ConstantesMetodos} from '../app/Service/Constantes';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';

declare var bootstrap: any;

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,    
    FormsModule,
    CommonModule,
    HttpClientModule,
    MatDatepickerModule,
    MatInputModule,
    MatNativeDateModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',

})

export class AppComponent {
  private URL: ConstantesURL;
  urlBase: string;
  private Metodos : ConstantesMetodos;
  endpoint: string; 
  constructor(private http: HttpClient) { 
    this.URL = new ConstantesURL;
    this.Metodos = new ConstantesMetodos;
    this.urlBase = this.URL.URLBack;
    this.endpoint = this.Metodos.ContactoUsuario;
  }

  

  @ViewChild('modal') modal!: ElementRef;

  title = 'GreenLeaves';  
  TXTLocacion : string = '';
  TXTFecha : string = '';
  TXTTelefono : string = '';
  TXTEmail : string = '';
  TXTNombre : string = '';
  BTNEnviar : any | undefined;

  selectedDate :any;

  ResponseContactoUsuario : any;
  ResponseContactoUsuarioError : any[] = [];

  nombreCompletos: string[] = []; 
  Localidades: any = null;
  ciudades: any[] = [];
  City : string = '';
  State : string = '';
  Country : string = ''; 
  localidadSplit: string[] = [];

  handleClick() {
    alert('¡Botón clickeado!');
  }

  openModal(modalId: string) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
    } else {
        console.error(`Modal con ID ${modalId} no encontrado`);
    }
}

closeModal(modalId: string) {
  const modalElement = document.getElementById(modalId);
  if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
          modal.hide();
      } else {
          console.error(`No se encontró una instancia del modal con ID ${modalId}`);
      }
  } else {
      console.error(`Modal con ID ${modalId} no encontrado`);
  }
}

  filterText: string = '';
  filteredItems: any[] = [...this.ciudades];
  filteredNombresCompletos: string[] = [];
  showDropdown: boolean = false;

  filterList(): void {

    if(this.filterText.length >= 3 ){
        var urlBase = this.URL.URLBack;
        var endpoint = this.Metodos.ContactoLocalidades;

        var url = urlBase + endpoint + "?str=" + this.filterText;
        this.http.get(url)
        .subscribe(          
          respuesta => { 
            this.ciudades = [];           
            this.Localidades = respuesta;
            if(!this.Localidades.isError){
              this.ciudades = this.Localidades.list.map((item: any) => ({
                nombreCompleto: `${item.cIudad}, ${item.estado}, ${item.pais}`,
              }));                  
            }
          },
          error => {
            console.log('Error en la solicitud:', error);

          }
        );        
        this.filteredItems = this.ciudades.filter(item =>
          item.nombreCompleto.toLowerCase().includes(this.filterText.toLowerCase())
        );
      
        this.filteredNombresCompletos = this.filteredItems.map(lugar => lugar.nombreCompleto);
    }
    else{
      this.filteredItems = this.ciudades;
    }    
  };

  getNombreCompleto(item: any): string[] {
    return item.nombreCompleto;
  }

  selectItem(item: string): void {
    this.filterText = item;
    this.showDropdown = false;
  };

  toggleDropdown(show: boolean): void {
    this.showDropdown = show;
  };

  @HostListener('document:click', ['$event'])
  clickOutside(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown')) {
      this.showDropdown = false;
    }
  };


  EnviarDatos(){

    const date = new Date(this.selectedDate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // +1 porque los meses son indexados desde 0
    const day = String(date.getDate()).padStart(2, '0');
    this.TXTFecha = `${year}-${month}-${day}`;

    this.localidadSplit = this.filterText.split(", ");
    this.City = this.localidadSplit[0];
    this.State = this.localidadSplit[1];
    this.Country = this.localidadSplit[2];
    const ContactoUsuario ={
      name: this.TXTNombre,
      email: this.TXTEmail,
      phone: this.TXTTelefono,
      date: this.TXTFecha,
      city: this.City,
      state: this.State,
      country: this.Country
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    var urlBase = this.URL.URLBack;
    var endpoint = this.Metodos.ContactoUsuario;
    var url = urlBase + endpoint;

    this.http.post(url , ContactoUsuario, { headers })
      .subscribe(
        (respuesta : any) => {
          console.log('Respuesta del servidor:', respuesta.message);
          this.ResponseContactoUsuario = respuesta.message;
          this.openModal('miModal');
          this.clearDatos();
        },
        (error: any) => {
          console.log('Error en la solicitud:', error);
          console.log(error.error.errors)

          for (const campo in error.error.errors) {
            if (error.error.errors.hasOwnProperty(campo)) {
                // Acceder al array de mensajes de cada campo
                const mensajes = error.error.errors[campo];
                // Agregar cada mensaje al array de mensajesDeError
                this.ResponseContactoUsuarioError.push(...mensajes);
            }
          }

          console.log(this.ResponseContactoUsuarioError)
          this.openModal('miModalError');
        }
      );
  }

  clearDatos(){
    this.TXTNombre = '';
    this.TXTEmail = '';
    this.TXTTelefono = '';
    this.selectedDate = '';
    this.filterText = '';
  }

  
}
