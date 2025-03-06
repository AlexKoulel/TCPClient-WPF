# TCPClient-WPF  
A simple TCP client application built using WPF and C#.  

## How to Test the Client  

### Option 1: Using a Python TCP Server  
1. Build and run the WPF TCP Client.  
2. Open a terminal and navigate to the project's directory.  
3. Run the following command:  
   ```python server_test.py```
4. On the WPF app type **127.0.0.1** in the IP address and **5000** in the port and press the connect button.
5. A welcoming message should appear on the WPF application.

### Option 2: Using HERCULES Setup Utility
Alternatively, you can use [HERCULES Setup Utility](https://www.hw-group.com/software/hercules-setup-utility)
1. Download and open HERCULES.
2. Navigate to the **TCP SERVER** tab.
3. Set a port (e.g. **5000**) and click listen.
4. On the WPF app type **127.0.0.1** in the IP address and **5000** in the port and press the connect button.
5. Back on the HERCULES application write a message and click *Send* to see the results on the WPF application.