import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { Component, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import {ConstantesURL}  from '../app/Service/Constantes';
import {ConstantesMetodos} from '../app/Service/Constantes';

declare var bootstrap: any;

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,    
    FormsModule,
    CommonModule,
    HttpClientModule
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

  City : string = '';
  State : string = '';
  Country : string = ''; 

  handleClick() {
    alert('¡Botón clickeado!');
  }

  openModal() {
    const modal = new bootstrap.Modal(this.modal.nativeElement);
    modal.show();
  }

  EnviarDatos(){
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
        respuesta => {
          console.log('Respuesta del servidor:', respuesta);
        },
        error => {
          console.log('Error en la solicitud:', error);
          
        }
      );
  }

  
}
