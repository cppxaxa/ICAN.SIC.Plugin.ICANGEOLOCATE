with open('IN_new.txt', 'w') as fout:
    with open('IN.txt', 'r', errors='ignore') as f:
        for line in f:
            cells = line.split('\t')

            for i in range(len(cells)):
                # print('O', end='')

                if i != 0:
                    fout.write('\t' + cells[i].strip())
                else:
                    fout.write('1112233')
            fout.write('\n')

        