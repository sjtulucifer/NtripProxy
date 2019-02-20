import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Company } from '../entities/company';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' })
};

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  private baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.webApiUrl + 'Company/';
  }

  addCompany(company: Company): Observable<any> {
    return this.http.post(this.baseUrl + 'AddCompany', company);
  }

  deleteCompany(id: string): Observable<any> {
    return this.http.put(this.baseUrl + 'SoftDeleteCompanyByID/' + id, null);
  }

  updateCompany(company: Company): Observable<any> {
    return this.http.put(this.baseUrl+ 'UpdateCompany', company);
  }

  updateParentCompany(obj: any): Observable<any> {
    return this.http.put(this.baseUrl + 'UpdateParentCompany', obj);
  }

  getCompanyList(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAllCompanies');
  }

  getCompanyByID(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetCompanyByID/' + id);
  }

  getOtherCompanies(id: string): Observable<any> {
    return this.http.get(this.baseUrl + 'GetOtherCompanies/' + id);
  }
}
