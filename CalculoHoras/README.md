# Descrição do Aplicativo
O aplicativo "SACH" (Software de auxilio para o calculo de horas) é um aplicativo de console em C# que lê arquivos CSV de uma pasta informada pelo usuário e os converte para um único arquivo JSON.

O programa requer que os arquivos CSV tenham um cabeçalho com a ordem dos campos distribuídos e que o texto esteja codificado em UTF-8.

# Utilização
Ao iniciar o aplicativo, será solicitado ao usuário que informe o caminho da pasta que contém os arquivos CSV a serem processados. O programa irá listar todos os arquivos CSV presentes na pasta, processar o conteúdo e então gerará um único arquivo JSON.

# Requisitos
Para utilizar o programa, é necessário ter o .NET 6 ou superior instalado na máquina. Além disso, os arquivos CSV devem seguir o seguinte formato:

* Cabeçalho: o arquivo deve possuir um cabeçalho com a ordem dos campos distribuídos.
* Codificação de Texto: o texto do arquivo deve estar codificado em UTF-8.

## Exemplo de Cabeçalho 
Para que o programa possa ler corretamente os arquivos CSV, é importante que o cabeçalho siga a ordem dos campos distribuídos. Abaixo segue um exemplo de cabeçalho:
```
Código;Nome;Valor hora;Data;Entrada;Saída;Almoço
```

# Autor
Esse aplicativo foi desenvolvido por Dennis Fernando Rupel.