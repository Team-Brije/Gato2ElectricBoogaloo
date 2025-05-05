const WebSocket = require('ws');
const clients = [];
const users = [];

const httpPort = 80;
const websockPort = 8080;

var fs = require('fs');
var directory = "./Chats/";


class User{
    constructor()
    {
        this._username="No name";
        this._conn=null;
    }

    set username( user )
    {
        this._username = user;
    }

    get username ()
    {
        return this._username;
    }

    set connection( con )
    {
        this._conn = con;
    }

    get connection ()
    {
        return this._conn;
    }

    static findClientByUsername (lst, username)
    {
        lst.forEach(user => {
            if(user.username === username)
            {
                return user;
            }
        });
        return null;
    }
}

const wss = new WebSocket.Server({ port: 8080 },()=>{
    console.log('Server Started');
});

wss.on('connection', function connection(ws) {
    
    console.log('New connenction');
    let user = new User ();
    user.connection = ws;
    users.push(user); // Agregar la conexión (cliente) a la lista
    
    ws.on('open', (data) => {
        console.log('Now Open');
    });

    ws.on('message', (data) => {
        console.log('Data received: %s',data);
        
        //ws.send("The server response: "+data); // Para mandar el mensaje al cliente que lo envió

        let info = data.toString().split('|'); // 200|username // 100| // 300|id|pos

        switch (info[0])
        {
            case '1': // set Username
                user.username = info[1];
                user.connection.send("1|Username Set: "+user.username);
            break; 

            case '2': // getList
                let lista = "";
                users.forEach(us => {
                    if(us.connection.readyState === WebSocket.OPEN)
                    {
                        lista = lista + us.username + "-";
                        //us.send(cliente.username + " says: " + data); // si falla, cambiar a: `data.toString()`
                    }
                });
                user.connection.send('2|'+lista);
            break;

            case '3': // Handshake
                let u=true;

                users.forEach(us => {
                    if(us.username === info[1])
                    {
                        u=false;
                        us.connection.send("3|"+info[2]);
                    }
                });

                if(u == true){
                    user.connection.send("404|User not found");
                }

                break;

                case '4': // Answer
                let ua=true;

                users.forEach(us => {
                    if(us.username === info[1])
                    {
                        ua=false;
                        us.connection.send("4|"+info[1]+"|"+info[3]);
                    }
                });

                if(ua == true){
                    user.connection.send("404|User not found");
                }

                break;

                case '5': // Check if it exists

                let chatexists = true
                let player1 = info[1];
                let player2 = info[2];

                var chats = fs.readdirSync(directory);

                for (const element of chats) {
                    let tempchat = fs.readFileSync(directory+element,"utf-8");
                    let words = JSON.parse(tempchat);
                    let data = words.data;
                    let player1temp = data[1];
                    let player2temp = data[2];
                    
                    if(player1temp === player1 && player2temp === player2)
                    {
                        let filedir = directory + element;
                        chatexists = true;
                        console.log("CHAT FOUND");
                        users.forEach(us => {
                            if(us.username === player1)
                            {
                                ub=false;
                                us.connection.send("5|"+filedir);
                            }
                        });
                        users.forEach(us => {
                            if(us.username === player2)
                            {
                                ub=false;
                                us.connection.send("5|"+filedir);
                            }
                        });
                        break;
                    }
                    else if(player1temp === player2 && player2temp === player1)
                    {
                        let filedir = directory + element;
                        chatexists = true;
                        console.log("CHAT FOUND");
                        users.forEach(us => {
                            if(us.username === player1)
                            {
                                ub=false;
                                us.connection.send("5|"+filedir);
                            }
                        });
                        users.forEach(us => {
                            if(us.username === player2)
                            {
                                ub=false;
                                us.connection.send("5|"+filedir);
                            }
                        });
                        break;
                    }
                    else 
                    {
                        chatexists = false;
                    }
                }
                if (chatexists == false)
                {
                    let number = 0;
                    number = chats.length+1;
                    let newJSON = {
                        "data":[number,player1,player2],
                        "chats":[]
                    };
                    let datamoment = JSON.stringify(newJSON)
                    fs.writeFileSync(directory + number + ".json",datamoment);
                    let newfiledir = directory + number + ".json";
                    users.forEach(us => {
                        if(us.username === player2)
                        {
                            ub=false;
                            us.connection.send("5|"+newfiledir);
                        }
                    });
                    users.forEach(us => {
                        if(us.username === player1)
                        {
                            ub=false;
                            us.connection.send("5|"+newfiledir);
                        }
                    });
                }
                break;

                case '6':

                direc = info[1];

                let chatmoment = fs.readFileSync(direc, 'utf-8');

                let datafromChat = JSON.parse(chatmoment);

                let chatData = datafromChat.chats;

                user.connection.send('6|'+chatData);

                break;

                case '7':

                newMsg = info[3]

                direc2 = "./Chats/1.json";

                let chatmoment2 = fs.readFileSync("./Chats/1.json", 'utf-8');

                let datafromChat2 = JSON.parse(chatmoment2);
                
                datafromChat2.chats = datafromChat2.chats+"₡"+info[1]+"₡"+newMsg;

                let newJSON = JSON.stringify(datafromChat2)
                
                fs.writeFileSync(direc2,newJSON);

                users.forEach(us => {
                    if(us.username === info[1])
                    {
                        us.connection.send("6|"+info[2]);
                    }
                });

                    /*
                    console.log(info[1]);
                    let direcMsg = "./Chats/1.json";
                    let chatrecived = fs.readFileSync(direcMsg, 'utf-8');

                    let chatgetter = JSON.parse(chatrecived);
                    let chata = chatgetter.chats;
                    console.log(chata)*/
                    /*
                    let chatmoment2 = fs.readFileSync(direcMsg, 'utf-8');
                    let datafromChat2 = JSON.stringify(chatmoment2);
                    */
                    //console.log(chatmoment2)
                    //console.log(info[1]);
                    //console.log(datafromChat2)
                    
                
                    

                    break;

                
                case '404': // Mandar mensaje directo
                break;

            default: // broadcast
                // Mandar a todos los clientes conectados el mensaje con el username de quien lo envió
                users.forEach(us => {
                    if(us.readyState === WebSocket.OPEN)
                    {
                        us.send(us.username + " says: " + data); // si falla, cambiar a: `data.toString()`
                    }
                });
            break;
        }
    });

    // Al cerrar la conexión, quitar de la lista de clientes
    ws.on('close', () => { 
        let index = users.indexOf(user);
        if(index > -1)
        {
            users.splice(index, 1);
            user.connection.send("UserName disconnected: "+user.username);
        }
    });
});

wss.on('listening',()=>{
   console.log('Now listening on port 8080...');
});


/*
const express = require ('express');
const app = express();
app.use(express.json());
app.use(express.urlencoded({ extended: true }));


app.get('/', (req, res) => {
    let str = "<h1>Versión web</h1>";
    res.send(str);
});

app.get('/getusers', (req, res) => {
    let lista = "<ul>";
    users.forEach(us => {
        if(us.connection.readyState === WebSocket.OPEN)
        {
            lista += "<li>" + us.username + "</li>";
        }
    });
    lista += "</ul>";
    res.send(lista);
});

app.get('/sendmessage', (req, res) => {
    let lista = "<ul>";
    users.forEach(us => {
        if(us.connection.readyState === WebSocket.OPEN)
        {
            lista += "<li>" + us.username + "</li>";
        }
    });
    lista += "</ul>";

    let page = "<html><head><title>Send Message</title></head><body>"+lista+"<form action='/sendmessage' method='post'><label>to:</label><input type='text' name='to'/><br><label>from:</label><input type='text' name='from'/><br><label>message:</label><input type='text' name='message'/><br><br><input type='submit' value='enviar'/></body></html>";

    res.send(page);
});

app.post('/sendmessage', (req, res) => { // to, from, message
    let form_to = req.body.to;
    let form_from = req.body.from;
    let form_mess = req.body.message;

    let u=true;
    let page = ":)"
                users.forEach(us => {
                    if(us.username === form_to)
                    {
                        u=false;
                        console.log("User Found!")
                        us.connection.send("400| Message from "+form_from+": "+form_mess);
                    }
                });
                console.log(users)
                if(u == true){
                    page = "<html><head><title>Message Sent</title></head><body><h1>404 USER NOT FOUND</h1></body></html>";
                }
                else{
                    page = "<html><head><title>Message Sent</title></head><body><h1>Message Sent!</h1></body></html>";
                }
    
    res.send(page);
});

app.listen(httpPort, () => {
    console.log(`HTTPServer init in: ${httpPort}`);
});

// app.get('/status/:player', (req, res) => {}); // http://localhost/status/1 // req.params['player']
*/