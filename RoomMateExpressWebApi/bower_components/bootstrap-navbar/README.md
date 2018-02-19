# bootstrap-navbar
Bootstrap navigation polymer1.0 component

### Usage

Include bootstrap-navbar.html

```sh
<bootstrap-navbar menu-type="navbar-fixed-top" submit-value="Login" is-form=true form-position="right" brand-name="Bootstrap-Menu" brand-url="https://www.google.com/">
        <li><a href="#">Link One</a></li>
        <li><a href="#">Link Two</a></li>
        <li><a href="#">Link Three</a></li>
    </bootstrap-navbar>
```
Below are the list of attributes for the element:

Menu types:
```sh
Static Top (menu-type= 'navbar-static-top')
Fixed Top (menu-type= 'navbar-fixed-top')
```
Brand:
```sh
Website name you want to display (brand-name= "Bootstrap-Menu")
Website url you want to point to (brand-url= "https://www.google.com/")
```
Search:
```sh
To display search form in the menu (is-form= true)
Form position in the menu (form-position= "right")
Form submit value to display (submit-value= "Login")
```
Is Bootstrap - Are you using bootstrap in your project already then true else false
```sh
Bootstrap not there in your application (is-bootstrap= false) 

default is true
```

Note: Dependency on Jquery.

