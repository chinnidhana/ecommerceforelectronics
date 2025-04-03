import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ProductsComponent } from './products/products.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent  // Declare your ProductsComponent here
  ],
  imports: [
    BrowserModule,
    HttpClientModule  // Import HttpClientModule for HTTP communication
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
