import { NgModule } from '@angular/core';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatIconModule} from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatListModule} from '@angular/material/list';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatDividerModule} from '@angular/material/divider';
import {MatCardModule} from '@angular/material/card';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';

@NgModule({
    exports:[
        MatSidenavModule,
        MatIconModule,
        MatToolbarModule,
        MatListModule,
        MatGridListModule,
        MatButtonModule,
        MatInputModule,
        MatDividerModule,
        MatCardModule,
        MatProgressSpinnerModule,
        MatFormFieldModule,
        MatSelectModule
    ]
  })
  export class MaterialModule { }